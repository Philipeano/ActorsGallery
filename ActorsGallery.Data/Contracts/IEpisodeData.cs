using ActorsGallery.Core.DTOs;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface IEpisodeData
    {
        // Mandatory methods as stated in requirements doc
        public List<EpisodeDTO> GetAllEpisodes();

        public List<EpisodeDTO> SearchByCharacter(string searchKey, string searchValue);

        public void AddComment(string episodeId, CommentDTO input, string commenterIpAddress, out string responseMsg);


        // Extra methods necessary for CRUD operations
        public void AddCharacter(string episodeId, EpisodeCharacterDTO input, out string responseMsg);

        public EpisodeDTO CreateEpisode(EpisodeDTO input, out string responseMsg);

        public EpisodeDTO UpdateEpisode(string episodeId, EpisodeDTO input, out string responseMsg);

        public void DeleteEpisode(string episodeId, out string responseMsg);
    }
}
