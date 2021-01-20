using ActorsGallery.Core.DTOs;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ICharacterData
    {
        // Mandatory methods as stated in requirements doc
        public List<CharacterDTO> GetCharacters(string filterKey, string filterValue, string sortKey, string sortOrder);


        // Extra methods necessary for CRUD operations
        public CharacterDTO CreateCharacter(CharacterRequestBody input, out string responseMsg);


        public CharacterDTO UpdateCharacter(long characterId, CharacterRequestBody input, out string responseMsg);


        public void DeleteCharacter(long characterId, out string responseMsg);
    }
}
