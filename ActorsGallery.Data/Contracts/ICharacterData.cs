using ActorsGallery.Core.Models;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ICharacterData
    {
        public IEnumerable<Character> GetAllCharacters(string sortKey, string sortDirection);

        public IEnumerable<Character> GetCharactersByFilter(string filterKey, string filterValue);
    }
}
