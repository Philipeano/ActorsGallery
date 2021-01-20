using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.LookUp;
using ActorsGallery.Data.Contracts;
using System;
using System.Collections.Generic;

namespace ActorsGallery.Data.Utilities
{
    public class Validator
    {
        private readonly List<string> validGenderValues = GenderOptions.Values;
        private readonly List<string> validStatusValues = StatusOptions.Values;
        private readonly List<string> validNigerianStates = NigerianStates.Values;

        private readonly List<string> validSortKeys = new List<string> { "firstname", "lastname", "gender" };
        private readonly List<string> validSortOrders = new List<string> { "asc", "desc", "ascending", "descending" };
        private readonly List<string> validFilterKeys = new List<string> { "gender", "status", "location" };

        private readonly IFetcher fetcher;

        public Validator(IFetcher fetcherService)
        {
            fetcher = fetcherService;
        }

        public bool IsValidEntry(string entryName, string entryValue)
        {
            switch (entryName)
            {
                case "gender":
                    if (entryValue == null || entryValue == string.Empty ||
                        (validGenderValues.FindIndex(x => x.Equals(entryValue, StringComparison.OrdinalIgnoreCase)) == -1))
                        return false;
                    break;

                case "status":
                    if (entryValue == null || entryValue == string.Empty ||
                        (validStatusValues.FindIndex(x => x.Equals(entryValue, StringComparison.OrdinalIgnoreCase)) == -1))
                        return false;
                    break;

                case "origin":
                    if (entryValue == null || entryValue == string.Empty)
                        return true;
                    else if (validNigerianStates.FindIndex(x => x.Equals(entryValue, StringComparison.OrdinalIgnoreCase)) == -1)
                        return false;
                    break;
            }
            return true;
        }


        public bool IsValidParam(string type, string value)
        {
            switch (type)
            {
                case "sortkey":
                    if (value == null || value == string.Empty ||
                        (validSortKeys.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)) == -1))
                        return false;
                    break;

                case "sortorder":
                    if (value == null || value == string.Empty ||
                        (validSortOrders.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)) == -1))
                        return false;
                    break;

                case "filterkey":
                    if (value == null || value == string.Empty ||
                        (validFilterKeys.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)) == -1))
                        return false;
                    break;
            }
            return true;
        }


        public bool ValidateCharacterObj(CharacterRequestBody input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
                return false;
            }
            else
            {
                errorMsg = string.Empty;

                if (input.FirstName == string.Empty || input.LastName == string.Empty)
                    errorMsg = "Both 'First Name' and 'Last Name' are required. <br />";

                if (!IsValidEntry("status", input.Status))
                    errorMsg = $"{errorMsg}'Status' must be any of 'Active', 'Dead' or 'Unknown'. <br />";

                if (!IsValidEntry("origin", input.StateOfOrigin))
                    errorMsg = $"{errorMsg}'State of Origin', if provided, must be the name of a Nigerian state. <br />";

                if (!IsValidEntry("gender", input.Gender))
                    errorMsg = $"{errorMsg}'Gender' must be either 'Male' or 'Female'. <br />";

                if (input.LocationId == null || !long.TryParse(input.LocationId, out long valLocationId) || valLocationId < 1)
                    errorMsg = $"{errorMsg}'Location Id' must be a positive integer. <br />";
                else if (fetcher.FetchLocationById(valLocationId) == null)
                    errorMsg = $"{errorMsg}'Location Id' does not match any existing location. <br />";

                return errorMsg.Trim() == string.Empty;
            }
        }


        public bool ValidateEpisodeObj(EpisodeRequestBody input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
                return false;
            }
            else
            {
                errorMsg = string.Empty;

                if (input.Name == string.Empty)
                    errorMsg = "'Episode Name' is required. <br />";
                else if (fetcher.IsAlreadyUsed("name", input.Name))
                    errorMsg = "'Episode Name' is already in use. <br />";

                if (input.EpisodeCode == string.Empty)
                    errorMsg = $"{errorMsg}'Episode Code' is required. <br />";
                else if (fetcher.IsAlreadyUsed("code", input.EpisodeCode))
                    errorMsg = $"{errorMsg}'Episode Code' is already in use. <br />";

                try
                {
                    DateTime valReleaseDate = DateTime.Parse(input.ReleaseDate);
                }
                catch
                {
                    errorMsg = $"{errorMsg}'Release Date' is not a valid date. <br />";
                }

                return errorMsg.Trim() == string.Empty;
            }
        }


        public bool ValidateEpisodeId(string episodeId, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (episodeId == null || !long.TryParse(episodeId, out long valEpisodeId) || valEpisodeId < 1)
                errorMsg = "'Episode Id' must be a positive integer. <br />";
            else if (fetcher.FetchEpisodeById(valEpisodeId) == null)
                errorMsg = "'Episode Id' does not match any existing episode. <br />";

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateCommentObj(CommentRequestBody input, string ipAddress, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
            }
            else
            {
                errorMsg = string.Empty;

                if (input.CommentText == string.Empty)
                    errorMsg = $"{errorMsg}'Comment Text' is required. Up to 250 characters allowed. <br />";

                if (input.CommenterName == string.Empty)
                    errorMsg = $"{errorMsg}'Commenter's Name' is required. <br />";

                if (ipAddress == string.Empty)
                    errorMsg = $"{errorMsg}'Commenter's IP Address' cannot be determined. <br />";
            }

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateEpisodeCharacterObj(EpisodeRoleRequestBody input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
            }
            else
            {
                errorMsg = string.Empty;

                if (input.CharacterId == null || !long.TryParse(input.CharacterId, out long valCharacterId) || valCharacterId < 1)
                    errorMsg = "'Character Id' must be a positive integer. <br />";
                else if (fetcher.FetchCharacterById(valCharacterId) == null)
                    errorMsg = "'Character Id' does not match any existing character. <br />";

                if (input.RoleName == string.Empty)
                    errorMsg = $"{errorMsg}'Assigned Role' is required. Specify the role name. <br />";
            }

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateLocationObj(LocationRequestBody input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
                return false;
            }
            else
            {
                errorMsg = string.Empty;

                if (input.Name == string.Empty)
                    errorMsg = "'Location Name' is required. <br />";

                if (!double.TryParse(input.Latitude, out double valLatitude))
                    errorMsg = $"{errorMsg}'Latitude' must be a valid double-precision number. <br />";
                else if (valLatitude < -90 || valLatitude > 90)
                    errorMsg = $"{errorMsg}'Latitude' must be in the range -90 to +90. <br />";

                if (!double.TryParse(input.Longitude, out double valLongitude))
                    errorMsg = $"{errorMsg}'Longitude' must be a valid double-precision number. <br />";

                if (valLongitude < -180 || valLongitude > 180)
                    errorMsg = $"{errorMsg}'Longitude' must be in the range -180 to +180. <br />";

                return errorMsg.Trim() == string.Empty;
            }
        }


        public bool IsValidQuery(string query, out string[] parts, out string errorMsg)
        {
            parts = new string[] { };

            if (query == string.Empty)
            {
                errorMsg = "Your query is blank. Kindly use the required format.";
                return false;
            }
            else {
                string[] queryParts = query.Split(':');

                if(queryParts.Length != 2)
                {
                    errorMsg = "Your query is invalid. Kindly use the required format.";
                    return false;
                }

                if (queryParts[0] != "char_id" && queryParts[0] != "char_name")
                {
                    errorMsg = "Your query is invalid. Kindly use the required format.";
                    return false;
                }

                if (queryParts[0] == "char_id" && (!int.TryParse(queryParts[1], out int valCharId) || valCharId < 1))
                {
                    errorMsg= "Your query is invalid. <br />The 'id' value must be a positive integer.";
                    return false;

                }
                else
                {
                    parts = new string[] { queryParts[0], queryParts[1] };
                    //queryParts.CopyTo(parts, 0);
                    errorMsg = string.Empty;
                    return true;
                }
            }
        }
    }
}