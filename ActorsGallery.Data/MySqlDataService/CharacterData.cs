using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.LookUp;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActorsGallery.Data.MySqlDataService
{
    public class CharacterData : ICharacterData
    {

        private readonly ActorsGalleryContext context;

        private readonly List<string> validGenderValues = GenderOptions.Values;
        private readonly List<string> validStatusValues = StatusOptions.Values;
        private readonly List<string> validNigerianStates = NigerianStates.Values;

        private readonly List<string> validSortKeys = new List<string> { "firstname", "lastname", "gender" };
        private readonly List<string> validSortOrders = new List<string> { "asc", "desc", "ascending", "descending" };
        private readonly List<string> validFilterKeys = new List<string> { "gender", "status", "location" };


        public CharacterData(ActorsGalleryContext dbContext)
        {
            context = dbContext;
        }


        private Character GetCharacterById(long id)
        {
            return context.Characters
                 .Include(c => c.Location)
                 .SingleOrDefault(c => c.Id == id);
        }


        private List<Character> FetchAllCharacters()
        {
            return context.Characters
                 .Include(c => c.Location).ToList();
        }


        private bool IsValidOption(string value, List<string> validOptions, bool allowsNull = false)
        {
            if ((value == null || value == string.Empty) && allowsNull == false)
            {
                return false;
            }
            else if (!validOptions.Contains(value.ToLower()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private Location FindLocationById(string strLocationId)
        {
            if (!long.TryParse(strLocationId, out long valLocationId) || valLocationId < 1)
            {
                return null;
            }
            else
            {
                return context.Locations.Find(valLocationId);
            }
        }


        private bool ValidateInput(CharacterDTO input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
                return false;
            }

            else
            {
                errorMsg = String.Empty;

                if (input.FirstName == string.Empty || input.LastName == string.Empty)
                    errorMsg = "Both 'First Name' and 'Last Name' are required. \n";

                if (!IsValidOption(input.Status, validStatusValues, false))
                    errorMsg = $"{errorMsg}'Status' must be any of 'Active', 'Dead' or 'Unknown'. \n";

                if (!IsValidOption(input.StateOfOrigin, validNigerianStates, true))
                    errorMsg = $"{errorMsg}'State of Origin' must be a valid Nigerian state. \n";

                if (!IsValidOption(input.Gender, validGenderValues, false))
                    errorMsg = $"{errorMsg}'Gender' must be either 'Male' or 'Female'. \n";

                return errorMsg.Trim() == string.Empty;
            }
        }


        public List<CharacterDTO> GetCharacters(string filterKey = "none", string filterValue = "none", string sortKey = "default", string sortOrder = "default")
        {
            // Fetch all available characters 
            List<Character> characters = FetchAllCharacters();

            if (filterKey.ToString() != "none" && filterKey.ToString() != "none")
            {
                // Apply filter on result set, using 'filterkey' and 'filterValue' specified by user  
                characters = FilterCharacters(characters, filterKey, filterValue);
            }

            if (sortKey.ToString() == "default" && sortOrder.ToString() == "default")
            {
                // Sort result set by the 'Id' column by default
                characters = characters.OrderBy(c => c.Id).ToList();
            }
            else
            {
                // Sort result set by the 'sortKey' and 'sortValue' specified by user  
                characters = SortCharacters(characters, filterKey, filterValue);
            }

            // Render results using public-facing DTOs, rather than internal data representation 
            List<CharacterDTO> resultSet = new List<CharacterDTO> { };
            foreach (var character in characters)
            {
                resultSet.Add(new CharacterDTO
                {
                    Id = character.Id,
                    FirstName = character.FirstName,
                    LastName = character.LastName,
                    Status = character.Status,
                    StateOfOrigin = character.StateOfOrigin,
                    Gender = character.Gender,
                    LocationId = (character.Location != null) ? character.Location.Id.ToString() : string.Empty,
                    LocationName = (character.Location != null) ? character.Location.Name : string.Empty,
                    Created = character.Created
                });
            }
            return resultSet;
        }


        // Allow sorting by FirstName, LastName or Gender
        public List<Character> SortCharacters(List<Character> characters, string sortKey, string sortOrder)
        {
            if (!IsValidOption(sortKey, validSortKeys) || !IsValidOption(sortOrder, validSortOrders))
                return characters;
            else
            {
                switch (sortKey)
                {
                    case "firstname":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            characters.OrderBy(c => c.FirstName);
                        else
                            characters.OrderByDescending(c => c.FirstName);
                        break;
                    case "lastname":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            characters.OrderBy(c => c.LastName);
                        else
                            characters.OrderByDescending(c => c.LastName);
                        break;
                    case "gender":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            characters.OrderBy(c => c.Gender);
                        else
                            characters.OrderByDescending(c => c.Gender);
                        break;
                }
                return characters;
            }
        }


        // Allow filtering by Gender, Status or Location
        public List<Character> FilterCharacters(List<Character> characters, string filterKey, string filterValue)
        {
            if (!IsValidOption(filterKey, validFilterKeys) || filterValue == null || filterValue == string.Empty)
                return characters;
            else
            {
                switch (filterKey)
                {
                    case "gender":
                        characters.Where(c => c.Gender == filterValue);
                        break;
                    case "status":
                        characters.Where(c => c.Status == filterValue);
                        break;
                    case "location":
                        characters.Where(c => c.Location.Name == filterValue);
                        break;
                }
                return characters;
            }
        }


        public CharacterDTO CreateCharacter(CharacterDTO input, out string responseMsg)
        {
            if (ValidateInput(input, out responseMsg) == true)
            {
                // Validation checks passed. Create new Character record
                Character newCharacter = new Character
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Status = input.Status,
                    StateOfOrigin = input.StateOfOrigin,
                    Gender = input.Gender,
                    Created = DateTime.UtcNow,
                    Location = FindLocationById(input.LocationId)
                };
                context.Characters.Add(newCharacter);
                context.SaveChanges();


                return new CharacterDTO
                {
                    Id = newCharacter.Id,
                    FirstName = newCharacter.FirstName,
                    LastName = newCharacter.LastName,
                    Status = newCharacter.Status,
                    StateOfOrigin = newCharacter.StateOfOrigin,
                    Gender = newCharacter.Gender,
                    LocationId = (newCharacter.Location != null) ? newCharacter.Location.Id.ToString() : string.Empty,
                    LocationName = (newCharacter.Location != null) ? newCharacter.Location.Name : string.Empty,
                    Created = newCharacter.Created
                };
            }
            else
            {                
                return null;
            }
        }
    

        public CharacterDTO UpdateCharacter(long characterId, CharacterDTO input, out string responseMsg) 
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate characterId and input arguments
              - Retrieve matching character record, if any
              - Assign responseMsg a value, if necessary
              - Update the character record with the new values
              - Return updated character record
             */

            Character character = GetCharacterById(characterId);

            return new CharacterDTO { };
        }


        public void DeleteCharacter(long characterId, out string responseMsg) 
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate characterId
              - Delete matching character record, if any
              - Assign responseMsg a value, if necessary
             */
        }
    }
}