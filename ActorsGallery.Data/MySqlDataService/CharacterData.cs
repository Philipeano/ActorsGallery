using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class CharacterData : ICharacterData
    {
        private readonly ActorsGalleryContext context;
        private readonly IFetcher fetcher;
        private readonly Validator validator;

        public CharacterData(ActorsGalleryContext dbContext, IFetcher dataFetcher)
        {
            context = dbContext;
            fetcher = dataFetcher;
            validator = new Validator(dataFetcher);
        }


        public List<CharacterDTO> GetCharacters(string filterKey = "none", string filterValue = "none", string sortKey = "default", string sortOrder = "default")
        {
            // Fetch all characters 
            List<Character> characters = fetcher.FetchAllCharacters();

            if (filterKey.ToString() != "none")
            {
                // Apply filter on result set, using 'filterkey' and 'filterValue' specified by user  
                characters = FilterCharacters(characters, filterKey, filterValue);
                if (characters == null) return null;
            }

            if (sortKey.ToString() == "default" && sortOrder.ToString() == "default")
            {
                // Sort result set by the 'Id' column by default
                characters = characters.OrderBy(c => c.Id).ToList();
            }
            else
            {
                // Sort result set by the 'sortKey' and 'sortOrder' specified by user  
                characters = SortCharacters(characters, sortKey, sortOrder);
                if (characters == null) return null;
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
        private List<Character> SortCharacters(List<Character> characters, string sortKey, string sortOrder)
        {
            if (!validator.IsValidParam("sortkey", sortKey) || !validator.IsValidParam("sortorder", sortOrder))
                return null;
            else
            {
                switch (sortKey)
                {
                    case "firstname":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            return characters.OrderBy(c => c.FirstName).ToList();
                        else
                            return characters.OrderByDescending(c => c.FirstName).ToList();
                    case "lastname":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            return characters.OrderBy(c => c.LastName).ToList();
                        else
                            return characters.OrderByDescending(c => c.LastName).ToList();
                    case "gender":
                        if (sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending")
                            return characters.OrderBy(c => c.Gender).ToList();
                        else
                            return characters.OrderByDescending(c => c.Gender).ToList();
                }
                return characters;
            }
        }


        // Allow filtering by Gender, Status or Location
        private List<Character> FilterCharacters(List<Character> characters, string filterKey, string filterValue)
        {
            if (!validator.IsValidParam("filterkey", filterKey) || filterValue == null || filterValue == string.Empty)
                return null;
            else
            {
                switch (filterKey)
                {
                    case "gender":
                        return characters.Where(c => c.Gender.ToLower() == filterValue.ToLower()).ToList();
                    case "status":
                        return characters.Where(c => c.Status.ToLower() == filterValue.ToLower()).ToList();
                    case "location":
                        return characters.Where(c => c.Location.Name.ToLower() == filterValue.ToLower()).ToList();
                }
                return characters;
            }
        }


        public CharacterDTO CreateCharacter(CharacterDTO input, out string responseMsg)
        {
            if (!validator.ValidateCharacterObj(input, out responseMsg))
            {
                return null;
            }
            else
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
                    Location = fetcher.FetchLocationById(input.LocationId)
                };
                context.Characters.Add(newCharacter);
                context.SaveChanges();

                // Display new record using public-facing DTO, rather than internal data representation 
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

            Character character = fetcher.FetchCharacterById(characterId);

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