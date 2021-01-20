using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class EpisodeData : IEpisodeData
    {
        private readonly ActorsGalleryContext context;
        private readonly IFetcher fetcher;
        private readonly ICommentData commentData;
        private readonly Validator validator;

        public EpisodeData(ActorsGalleryContext dbContext, IFetcher dataFetcher, ICommentData commentService)
        {
            context = dbContext;
            fetcher = dataFetcher;
            validator = new Validator(dataFetcher);
            commentData = commentService;
        }


        public List<EpisodeDTO> GetAllEpisodes()
        {
            // Fetch all available episodes 
            List<Episode> episodes = fetcher.FetchAllEpisodes();

            // Render results using public-facing DTOs, rather than internal data representation 
            List<EpisodeDTO> resultSet = new List<EpisodeDTO> { };
            foreach (var episode in episodes)
            {
                resultSet.Add(new EpisodeDTO
                {
                    Id = episode.Id,
                    Name = episode.Name,
                    ReleaseDate = episode.ReleaseDate.ToString("D"),
                    EpisodeCode = episode.EpisodeCode,
                    NumOfCharacters = episode.EpisodeCharacters.Count,
                    NumOfComments = episode.EpisodeComments.Count,
                    Created = episode.Created
                });
            }
            return resultSet;
        }


        public List<EpisodeDTO> SearchByCharacter(string searchKey, string searchValue)
        {
            // Fetch all available roles in all episodes
            List<EpisodeCharacter> roles = new List<EpisodeCharacter> { };
            List<EpisodeDTO> resultSet = new List<EpisodeDTO> { };

            switch (searchKey)
            {                
                case "id": // Search by Character's Id
                    roles = fetcher.FetchAllRoles()
                        .Where(r => r.Character.Id.ToString() == searchValue)
                        .ToList();
                    break;                
                case "name": // Search by Character's First Name or Last Name
                    roles = fetcher.FetchAllRoles()
                        .Where(r => r.Character.FirstName.ToString() == searchValue.ToString() || 
                        r.Character.LastName.ToString() == searchValue.ToString()).ToList();
                    break;
            }

            // Render results using public-facing DTOs, rather than internal data representation 
            foreach (var role in roles)
            {
                resultSet.Add(new EpisodeDTO
                {
                    Id = role.EpisodeId,
                    Name = role.Episode.Name,
                    ReleaseDate = role.Episode.ReleaseDate.ToString("D"),
                    EpisodeCode = role.Episode.EpisodeCode,
                    NumOfCharacters = (role.Episode.EpisodeCharacters == null) ? 0 : role.Episode.EpisodeCharacters.Count,
                    NumOfComments = (role.Episode.EpisodeComments == null) ? 0 : role.Episode.EpisodeComments.Count,
                    Created = role.Episode.Created
                });
            }
            return resultSet;
        }


        public CommentDTO AddComment(string episodeId, CommentDTO input, string commenterIpAddress, out string responseMsg)
        {
            return commentData.AddCommentToEpisode(episodeId, input, commenterIpAddress, out responseMsg);
        }


        public EpisodeCharacterDTO AddCharacter(string episodeId, EpisodeCharacterDTO input, out string responseMsg)
        {
            bool validationResult1 = validator.ValidateEpisodeId(episodeId, out string validationMsg1);
            bool validationResult2 = validator.ValidateEpisodeCharacterObj(input, out string validationMsg2);
            bool validationResult3 = fetcher.RoleAlreadyAssigned(long.Parse(episodeId), long.Parse(input.CharacterId), out string validationMsg3);

            responseMsg = $"{validationMsg1}{validationMsg2}{validationMsg3}";

            if (!validationResult1 || !validationResult2 || validationResult3)
            {
                return null;
            }
            else
            {
                // Validation checks passed. Create new EpisodeCharacter record
                Episode episode = fetcher.FetchEpisodeById(long.Parse(episodeId));
                Character character = fetcher.FetchCharacterById(long.Parse(input.CharacterId));

                EpisodeCharacter newRole = new EpisodeCharacter
                {
                    Episode = episode,
                    Character = character,
                    RoleName = input.RoleName,
                    Created = DateTime.Now,
                };

                context.EpisodeCharacters.Add(newRole);
                context.SaveChanges();

                // Display new record using public-facing DTO, rather than internal data representation 
                return new EpisodeCharacterDTO
                {
                    EpisodeId = episode.Id.ToString(),
                    Name = episode.Name,
                    CharacterId = character.Id.ToString(),
                    CharacterName = $"{character.FirstName} {character.LastName}",
                    RoleName = newRole.RoleName,
                    Created = newRole.Created
                };
            }
        }


        public EpisodeDTO CreateEpisode(EpisodeDTO input, out string responseMsg)
        {
            if (validator.ValidateEpisodeObj(input, out responseMsg) == true)
            {
                // Validation checks passed. Create new Episode record
                Episode newEpisode = new Episode
                {
                    Name = input.Name,
                    EpisodeCode = input.EpisodeCode,
                    ReleaseDate = DateTime.Parse(input.ReleaseDate),
                    Created = DateTime.UtcNow,
                };
                context.Episodes.Add(newEpisode);
                context.SaveChanges();

                // Display new record using public-facing DTO, rather than internal data representation 
                return new EpisodeDTO
                {
                    Id = newEpisode.Id,
                    Name = newEpisode.Name,
                    EpisodeCode = newEpisode.EpisodeCode,
                    ReleaseDate = newEpisode.ReleaseDate.ToString("D"),
                    Created = newEpisode.Created,
                    NumOfCharacters = 0,
                    NumOfComments = 0               
                };
            }
            else
            {
                return null;
            }
        }


        public EpisodeDTO UpdateEpisode(string episodeId, EpisodeDTO input, out string responseMsg)
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate episodeId and input arguments
              - Retrieve matching episode record, if any
              - Assign responseMsg a value, if necessary
              - Update the episode record with the new values
              - Return updated episode record
             */

            return new EpisodeDTO { };
        }


        public void DeleteEpisode(string episodeId, out string responseMsg)
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate episodeId
              - Delete matching episode record, if any
              - Assign responseMsg a value, if necessary
             */
        }
    }
}