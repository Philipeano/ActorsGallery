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

        public Validator(IFetcher dataFetcher)
        {
            fetcher = dataFetcher;
        }


        public bool IsValidEntry(string entryName, string entryValue)
        {
            switch (entryName)
            {
                case "gender":
                    if (entryValue == null || entryValue == string.Empty || !validGenderValues.Contains(entryValue.ToLower()))
                        return false;
                    break;

                case "status":
                    if (entryValue == null || entryValue == string.Empty || !validStatusValues.Contains(entryValue.ToLower()))
                        return false;
                    break;

                case "origin":
                    if (entryValue == null || entryValue == string.Empty)
                        return true;
                    else if (!validNigerianStates.Contains(entryValue.ToLower()))
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
                    if (value == null || value == string.Empty || !validSortKeys.Contains(value.ToLower()))
                        return false;
                    break;

                case "sortorder":
                    if (value == null || value == string.Empty || !validSortOrders.Contains(value.ToLower()))
                        return false;
                    break;

                case "filterkey":
                    if (value == null || value == string.Empty || !validFilterKeys.Contains(value.ToLower()))
                        return false;
                    break;
            }
            return true;
        }


        public bool ValidateCharacterObj(CharacterDTO input, out string errorMsg)
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
                    errorMsg = "Both 'First Name' and 'Last Name' are required. \n";

                if (!IsValidEntry("status", input.Status))
                    errorMsg = $"{errorMsg}'Status' must be any of 'Active', 'Dead' or 'Unknown'. \n";

                if (!IsValidEntry("origin", input.StateOfOrigin))
                    errorMsg = $"{errorMsg}'State of Origin', if provided, must be the name of a Nigerian state. \n";

                if (!IsValidEntry("gender", input.Gender))
                    errorMsg = $"{errorMsg}'Gender' must be either 'Male' or 'Female'. \n";

                return errorMsg.Trim() == string.Empty;
            }
        }


        public bool ValidateEpisodeObj(EpisodeDTO input, out string errorMsg)
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
                    errorMsg = "'Episode Name' is required. \n";
                else if (fetcher.IsAlreadyUsed("name", input.Name))
                    errorMsg = "'Episode Name' is already in use. \n";

                if (input.EpisodeCode == string.Empty)
                    errorMsg = $"{errorMsg}'Episode Code' is required. \n";
                else if (fetcher.IsAlreadyUsed("code", input.EpisodeCode))
                    errorMsg = $"{errorMsg}'Episode Code' is already in use. \n";

                try
                {
                    DateTime valReleaseDate = DateTime.Parse(input.ReleaseDate);
                }
                catch
                {
                    errorMsg = $"{errorMsg}'Release Date' is not a valid date. \n";
                }

                return errorMsg.Trim() == string.Empty;
            }
        }


        public bool ValidateEpisodeId(string episodeId, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (episodeId == null || !long.TryParse(episodeId, out long valEpisodeId) || valEpisodeId < 1)
                errorMsg = "'Episode Id' must be a positive integer. \n";
            else if (fetcher.FetchEpisodeById(valEpisodeId) == null)
                errorMsg = "'Episode Id' does not match any existing episode. \n";

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateCommentObj(CommentDTO input, string ipAddress, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
            }
            else
            {
                errorMsg = string.Empty;

                if (input.CommentText == string.Empty)
                    errorMsg = $"{errorMsg}'Comment' is required. Up to 250 characters allowed. \n";

                if (input.CommenterName == string.Empty)
                    errorMsg = $"{errorMsg}'Commenter's Name' is required. \n";

                if (ipAddress == string.Empty)
                    errorMsg = $"{errorMsg}'Commenter's IP Address' cannot be determined. \n";
            }

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateEpisodeCharacterObj(EpisodeCharacterDTO input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
            }
            else
            {
                errorMsg = string.Empty;

                if (input.CharacterId == null || !long.TryParse(input.CharacterId, out long valCharacterId) || valCharacterId < 1)
                    errorMsg = "'Character Id' must be a positive integer. \n";
                else if (fetcher.FetchCharacterById(valCharacterId) == null)
                    errorMsg = "'Character Id' does not match any existing character. \n";

                if (input.RoleName == string.Empty)
                    errorMsg = $"{errorMsg}'Assigned Role' is required. Specify the role name. \n";
            }

            return errorMsg.Trim() == string.Empty;
        }


        public bool ValidateLocationObj(LocationDTO input, out string errorMsg)
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
                    errorMsg = "'Location Name' is required. \n";

                if (double.TryParse(input.Latitude, out double valLatitude))
                    errorMsg = $"{errorMsg}'Latitude' must be a valid double-precision number. \n";

                if (valLatitude < -90 || valLatitude > 90)
                    errorMsg = $"{errorMsg}'Latitude' must be in the range -90 to +90. \n";

                if (double.TryParse(input.Longitude, out double valLongitude))
                    errorMsg = $"{errorMsg}'Longitude' must be a valid double-precision number. \n";

                if (valLongitude < -180 || valLongitude > 180)
                    errorMsg = $"{errorMsg}'Longitude' must be in the range -180 to +180. \n";

                return errorMsg.Trim() == string.Empty;
            }
        }
    }
}