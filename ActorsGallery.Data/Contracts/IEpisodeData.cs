using ActorsGallery.Core.Models;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface IEpisodeData
    {
        //Sort list by ReleaseDate in ascending order; Include NumOfComments in each object
        public IEnumerable<Episode> GetAllEpisodes();


        // Return list of episodes featuring specified character
        public IEnumerable<Episode> FindEpisodesByCharacter(string characterName);


        public void AddCommentToEpisode(long episodeId, Comment commentObj);
    }
}
