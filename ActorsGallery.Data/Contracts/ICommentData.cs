using ActorsGallery.Core.DTOs;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ICommentData
    {
        // Mandatory methods as stated in requirements doc
        public List<CommentDTO> GetAllComments();


        public List<CommentDTO> GetEpisodeComments(string episodeId);


        public CommentDTO AddCommentToEpisode(string episodeId, CommentDTO input, string ipAddress, out string responseMsg);


        // Extra methods necessary for CRUD operations
        public CommentDTO UpdateComment(string commentId, out string responseMsg);


        public void DeleteComment(string commentId, out string responseMsg);
    }
}
