using ActorsGallery.Core.Models;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ICommentData
    {
        // Sort the list by Created in descending order; Include CommenterIPAddress and Created properties
        public IEnumerable<Comment> GetAllComments();


        public IEnumerable<Comment> GetEpisodeComments(long episodeId);


        public Comment PostNewComment();
    }
}
