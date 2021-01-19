using ActorsGallery.Core.Models;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface IFetcher
    {
        // Fetch all characters
        public List<Character> FetchAllCharacters();


        // Fetch specific character 
        public Character FetchCharacterById(long id);


        // Fetch all episodes, sorted by ReleaseDate in ascending order 
        public List<Episode> FetchAllEpisodes();


        // Fetch all episodes featuring all characters, sorted by ReleaseDate in ascending order
        public List<Episode> FetchAllEpisodesWithAllCharacters();


        // Fetch specific episode
        public Episode FetchEpisodeById(long id);


        // Fetch all Comments, sorted by Created in descending order
        public List<Comment> FetchAllComments();


        // Fetch specific comment 
        public Comment FetchCommentById(long commentId);


        // Fetch all locations
        public List<Location> FetchAllLocations();


        // Fetch specific location 
        public Location FetchLocationById(long locationId);
        public Location FetchLocationById(string strLocationId);


        // Check if new episode detail is in use by another episode
        public bool IsAlreadyUsed(string field, string value);


        // Check if a character already has a role in an episode
        public bool RoleAlreadyAssigned(long episodeId, long characterId, out string errorMsg);
    }
}