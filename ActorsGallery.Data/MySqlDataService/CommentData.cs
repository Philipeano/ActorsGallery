using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class CommentData : ICommentData
    {

        private readonly ActorsGalleryContext context;

        public CommentData(ActorsGalleryContext dbContext)
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


        private List<Comment> FetchAllComments()
        {
            return context.Comments
                .Include(e => e.Episode)
                .OrderByDescending(e => e.Created)
                .ToList();
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
                    errorMsg = $"{errorMsg}'Commenter's IP Address' cannot be determined. \n";
            }

            return errorMsg.Trim() == string.Empty;
        }


        // Sort the list by Created in descending order; Include CommenterIPAddress and Created properties
        public List<CommentDTO> GetAllComments()
        {
            // Fetch all available comments 
            List<Comment> comments = FetchAllComments();

            // Render results using public-facing DTOs, rather than internal data representation 
            List<CommentDTO> resultSet = new List<CommentDTO> { };
            foreach (var comment in comments)
            {
                resultSet.Add(new CommentDTO
                {
                    Id = comment.Id,
                    CommentText = comment.CommentText,
                    CommenterName = comment.CommenterName,
                    IpAddress = comment.IpAddress,
                    EpisodeId = comment.EpisodeId.ToString(),
                    EpisodeName = comment.Episode.Name,
                    Created = comment.Created
                });
            }
            return resultSet;
        }


        public List<CommentDTO> GetEpisodeComments(string episodeId)
        {
            // Ensure specified Episode Id is valid
            bool validationResult = ValidateEpisodeId(episodeId, out string responseMsg);

            if (!validationResult)
                return null;

            // Fetch all available comments 
            List<Comment> comments = FetchAllComments()
                .Where(c => c.EpisodeId.ToString() == episodeId)
                .ToList();

            // Render results using public-facing DTOs, rather than internal data representation 
            List<CommentDTO> resultSet = new List<CommentDTO> { };
            foreach (var comment in comments)
            {
                resultSet.Add(new CommentDTO
                {
                    Id = comment.Id,
                    CommentText = comment.CommentText,
                    CommenterName = comment.CommenterName,
                    IpAddress = comment.IpAddress,
                    EpisodeId = comment.EpisodeId.ToString(),
                    EpisodeName = comment.Episode.Name,
                    Created = comment.Created
                });
            }
            return resultSet;
        }


        public CommentDTO AddCommentToEpisode(string episodeId, CommentDTO input, string ipAddress, out string responseMsg)
        {
            bool validationResult1 = ValidateEpisodeId(episodeId, out string validationMsg1);
            bool validationResult2 = ValidateCommentObj(input, ipAddress, out string validationMsg2);
            responseMsg = $"{validationMsg1}{validationMsg2}";

            if (!validationResult1 || !validationResult2)
            {
                return null;
            }
            else
            {
                Episode episode = FetchEpisodeById(long.Parse(episodeId));
                Comment newComment = new Comment
                {
                    Episode = episode,
                    CommentText = input.CommentText,
                    CommenterName = input.CommenterName,
                    IpAddress = ipAddress,
                    Created = DateTime.Now,
                };

                context.Comments.Add(newComment);
                context.SaveChanges();

                return new CommentDTO
                {
                    Id = newComment.Id,
                    CommentText = newComment.CommentText,
                    CommenterName = newComment.CommenterName,
                    IpAddress = newComment.IpAddress,
                    EpisodeId = newComment.EpisodeId.ToString(),
                    EpisodeName = newComment.Episode.Name,
                    Created = newComment.Created
                };
            }
        }


        public CommentDTO UpdateComment(string commentId, out string responseMsg) 
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate commentId and input arguments
              - Retrieve matching comment record, if any
              - Assign responseMsg a value, if necessary
              - Update the comment record with the new values
              - Return updated comment record
             */

            return new CommentDTO { };
        }


        public void DeleteComment(string commentId, out string responseMsg)
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate commentId
              - Delete matching comment record, if any
              - Assign responseMsg a value, if necessary
             */
        }
    }
}