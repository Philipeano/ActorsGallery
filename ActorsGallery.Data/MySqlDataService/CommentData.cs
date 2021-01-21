using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class CommentData : ICommentData
    {
        private readonly ActorsGalleryContext context;
        private readonly IFetcher fetcher;
        private readonly Validator validator;

        public CommentData(ActorsGalleryContext dbContext, IFetcher dataFetcher)
        {
            context = dbContext;
            fetcher = dataFetcher;
            validator = new Validator(dataFetcher);
        }


        public List<CommentDTO> GetAllComments()
        {
            // Fetch all comments 
            List<Comment> comments = fetcher.FetchAllComments();

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
            bool validationResult = validator.ValidateEpisodeId(episodeId, out string responseMsg);

            if (!validationResult)
            {
                return null;
            }
            else
            {
                // Fetch all comments for the episode
                List<Comment> comments = fetcher.FetchAllComments()
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
        }


        public CommentDTO AddCommentToEpisode(string episodeId, CommentRequestBody input, string ipAddress, out string responseMsg)
        {
            bool validationResult1 = validator.ValidateEpisodeId(episodeId, out string validationMsg1);
            bool validationResult2 = validator.ValidateCommentObj(input, ipAddress, out string validationMsg2);
            responseMsg = $"{validationMsg1}{validationMsg2}";

            if (!validationResult1 || !validationResult2)
            {
                return null;
            }
            else
            {
                // Validation checks passed. Create new Comment record
                Episode episode = fetcher.FetchEpisodeById(long.Parse(episodeId));
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

                // Display new record using public-facing DTO, rather than internal data representation 
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