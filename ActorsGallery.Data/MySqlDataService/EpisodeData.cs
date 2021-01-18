using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class EpisodeData : IEpisodeData
    {

        private readonly ActorsGalleryContext context;

        public EpisodeData(ActorsGalleryContext dbContext)
        {
            context = dbContext;
        }


        private Episode FetchEpisodeById(long id)
        {
            return context.Episodes
                 .Include(e => e.EpisodeCharacters)
                 .Include(e => e.EpisodeComments)
                 .SingleOrDefault(e => e.Id == id);
        }


        private List<Episode> FetchAllEpisodes()
        {
            // Sort by ReleaseDate in ascending order
            return context.Episodes
                .Include(e => e.EpisodeCharacters)
                .Include(e => e.EpisodeComments)
                .OrderBy(e => e.ReleaseDate)
                .ToList();
        }


        private Character FetchCharacterById(long id)
        {
            return context.Characters
                 .SingleOrDefault(e => e.Id == id);
        }


        private bool IsAlreadyUsed(string field, string value)
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


        private bool RoleAlreadyAssigned(long episodeId, long characterId, out string errorMsg)
        {
            bool result = context.EpisodeCharacters
                .Any(ec => ec.EpisodeId == episodeId && ec.CharacterId == characterId);

            errorMsg = result ? "This character already has a role in this episode. \n" : string.Empty;

            return result;
        }


        private bool ValidateEpisodeObj(EpisodeDTO input, out string errorMsg)
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
                else if (IsAlreadyUsed("name", input.Name))
                    errorMsg = "'Episode Name' is already in use. \n";

                if (input.EpisodeCode == string.Empty)
                    errorMsg = $"{errorMsg}'Episode Code' is required. \n";
                else if (IsAlreadyUsed("code", input.EpisodeCode))
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


        private bool ValidateEpisodeId(string episodeId, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (episodeId == null || !long.TryParse(episodeId, out long valEpisodeId) || valEpisodeId < 1)
                errorMsg = "'Episode Id' must be a positive integer. \n";
            else if (FetchEpisodeById(valEpisodeId) == null)
                errorMsg = "'Episode Id' does not match any existing episode. \n";

            return errorMsg.Trim() == string.Empty;
        }


        private bool ValidateCommentObj(CommentDTO input, string ipAddress, out string errorMsg)
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
                    errorMsg = $"{errorMsg}'Commenter's IP Address' cannot be obtained. \n";
            }

            return errorMsg.Trim() == string.Empty;
        }


        private bool ValidateEpisodeCharacterObj(EpisodeCharacterDTO input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
            }
            else
            {
                errorMsg = String.Empty;

                if (input.CharacterId == null || !long.TryParse(input.CharacterId, out long valCharacterId) || valCharacterId < 1)
                    errorMsg = "'Character Id' must be a positive integer. \n";
                else if (FetchCharacterById(valCharacterId) == null)
                    errorMsg = "'Character Id' does not match any existing character. \n";

                if (input.RoleName == string.Empty)
                    errorMsg = $"{errorMsg}'Assigned Role' is required. Specify the role name. \n";
            }

            return errorMsg.Trim() == string.Empty;
        }


        // Include NumOfComments in each object
        public List<EpisodeDTO> GetAllEpisodes()
        {
            // Fetch all available episodes 
            List<Episode> episodes = FetchAllEpisodes();

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


        // Return list of episodes featuring specified character
        public List<EpisodeDTO> SearchByCharacter(string searchKey, string searchValue)
        {
            List<Episode> episodes = context.Episodes
                        .Include(e => e.EpisodeCharacters)
                        .ThenInclude(ec => ec.Character)
                        .ToList();

            switch (searchKey)
            {
                // Search by Character's Id
                case "id": 
                    foreach (var episode in episodes)
                    {
                        episode.EpisodeCharacters
                            .Where(ec => ec.Character.Id.ToString() == searchValue);
                    }
                    break;

                // Search by Character's First Name or Last Name
                case "name":
                    foreach (var episode in episodes)
                    {
                        episode.EpisodeCharacters
                            .Where(ec => ec.Character.FirstName == searchValue || ec.Character.LastName == searchValue);
                    }
                    break;
            }

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


        public void AddComment(string episodeId, CommentDTO input, string commenterIpAddress, out string responseMsg)
        {
            bool validationResult1 = ValidateEpisodeId(episodeId, out string validationMsg1);
            bool validationResult2 = ValidateCommentObj(input, commenterIpAddress, out string validationMsg2);

            if (validationResult1 && validationResult2)
            {
                Episode episode = FetchEpisodeById(long.Parse(episodeId));
                Comment newComment = new Comment
                {
                    Episode = episode,
                    CommentText = input.CommentText,
                    CommenterName = input.CommenterName,
                    IpAddress = commenterIpAddress,
                    Created = DateTime.Now,
                };

                context.Comments.Add(newComment);
                context.SaveChanges();
            }

            responseMsg = $"{validationMsg1}{validationMsg2}";
        }


        public void AddCharacter(string episodeId, EpisodeCharacterDTO input, out string responseMsg)
        {
            bool validationResult1 = ValidateEpisodeId(episodeId, out string validationMsg1);
            bool validationResult2 = ValidateEpisodeCharacterObj(input, out string validationMsg2);
            bool validationResult3 = RoleAlreadyAssigned(long.Parse(episodeId), long.Parse(input.CharacterId), out string validationMsg3);

            if (validationResult1 && validationResult2 && validationResult3)
            {
                Episode episode = FetchEpisodeById(long.Parse(episodeId));
                Character character = FetchCharacterById(long.Parse(input.CharacterId));

                EpisodeCharacter newRole = new EpisodeCharacter
                {
                    Episode = episode,
                    Character = character,
                    RoleName = input.RoleName,
                    Created = DateTime.Now,
                };

                context.EpisodeCharacters.Add(newRole);
                context.SaveChanges();
            }

            responseMsg = $"{validationMsg1}{validationMsg2}{validationMsg3}";
        }


        public EpisodeDTO CreateEpisode(EpisodeDTO input, out string responseMsg)
        {
            if (ValidateEpisodeObj(input, out responseMsg) == true)
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
