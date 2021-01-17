using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ICharacterData
    {
        // Mandatory methods as stated in requirements doc
        public List<CharacterDTO> GetCharacters(string filterKey, string filterValue, string sortKey, string sortOrder);

        public List<Character> SortCharacters(List<Character> characters, string sortKey, string sortOrder);

        public List<Character> FilterCharacters(List<Character> characters, string filterKey, string filterValue);


        // Extra methods to be implemented later
        public Character CreateCharacter(CharacterDTO input, out string responseMsg);

        public Character UpdateCharacter(long characterId, CharacterDTO input, out string responseMsg);

        public void DeleteCharacter(long characterId, out string responseMsg);
    }
}
