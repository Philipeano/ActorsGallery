using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.MySqlDataService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.Utilities
{
    public class Fetcher : IFetcher
    {
        private readonly ActorsGalleryContext context;

        public Fetcher(ActorsGalleryContext dbContext) 
        {
            context = dbContext;
        }


        // Fetch all characters
        public List<Character> FetchAllCharacters()
        {
            return context.Characters
                 .Include(c => c.Location)
                 .ToList();
        }


        // Fetch specific character 
        public Character FetchCharacterById(long id)
        {
            return context.Characters
                 .Include(c => c.Location)
                 .SingleOrDefault(c => c.Id == id);
        }


        // Fetch all episodes, sorted by ReleaseDate in ascending order 
        public List<Episode> FetchAllEpisodes()
        {
            return context.Episodes
                .Include(e => e.EpisodeCharacters)
                .Include(e => e.EpisodeComments)
                .OrderBy(e => e.ReleaseDate)
                .ToList();
        }


        // Fetch all episodes featuring all characters, sorted by ReleaseDate in ascending order
        public List<Episode> FetchAllEpisodesWithAllCharacters()
        {
            return context.Episodes
                .Include(e => e.EpisodeCharacters)
                .ThenInclude(ec => ec.Character)
                .OrderBy(e => e.ReleaseDate)
                .ToList();
        }


        // Fetch specific episode
        public Episode FetchEpisodeById(long id)
        {
            return context.Episodes
                 .Include(e => e.EpisodeCharacters)
                 .Include(e => e.EpisodeComments)
                 .SingleOrDefault(e => e.Id == id);
        }


        // Fetch all Comments, sorted by Created in descending order
        public List<Comment> FetchAllComments()
        {
            return context.Comments
                .Include(e => e.Episode)
                .OrderByDescending(e => e.Created)
                .ToList();
        }


        // Fetch specific comment 
        public Comment FetchCommentById(long commentId)
        {
            return context.Comments
                .Include(e => e.Episode)
                .FirstOrDefault(c => c.Id == commentId);
        }


        // Fetch all locations
        public List<Location> FetchAllLocations()
        {
            return context.Locations.ToList();
        }


        // Fetch specific location 
        public Location FetchLocationById(long locationId)
        {
            return context.Locations.Find(locationId);
        }


        public Location FetchLocationById(string strLocationId)
        {
            if (!long.TryParse(strLocationId, out long valLocationId) || valLocationId < 1)
            {
                return null;
            }
            else
            {
                return FetchLocationById(valLocationId);
            }
        }


        public bool IsAlreadyUsed(string field, string value)
        {
            Episode matchingEpisode = null;

            switch (field)
            {
                case "code":
                    matchingEpisode = context.Episodes
                        .Where(e => e.EpisodeCode.ToLower() == value.ToLower())
                        .FirstOrDefault();
                    break;

                case "name":
                    matchingEpisode = context.Episodes
                        .Where(e => e.Name.ToLower() == value.ToLower())
                        .FirstOrDefault();
                    break;
            }

            return matchingEpisode != null;
        }


        public bool RoleAlreadyAssigned(long episodeId, long characterId, out string errorMsg)
        {
            bool result = context.EpisodeCharacters
                .Any(ec => ec.EpisodeId == episodeId && ec.CharacterId == characterId);

            errorMsg = result ? "This character already has a role in this episode. \n" : string.Empty;

            return result;
        }
    }
}