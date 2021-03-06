﻿using ActorsGallery.Core.DTOs;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface IEpisodeData
    {
        // Mandatory methods as stated in requirements doc
        public List<EpisodeDTO> GetAllEpisodes();

        public List<EpisodeDTO> SearchByCharacter(string searchKey, string searchValue);

        public CommentDTO AddComment(string episodeId, CommentRequestBody input, string commenterIpAddress, out string responseMsg);


        // Extra methods necessary for CRUD operations
        public EpisodeCharacterDTO AddCharacter(string episodeId, EpisodeRoleRequestBody input, out string responseMsg);

        public EpisodeDTO CreateEpisode(EpisodeRequestBody input, out string responseMsg);

        public EpisodeDTO UpdateEpisode(string episodeId, EpisodeRequestBody input, out string responseMsg);

        public void DeleteEpisode(string episodeId, out string responseMsg);
    }
}
