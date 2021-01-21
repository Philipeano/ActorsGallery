using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
    public class EpisodeCharacterDTO
    {
        [Required, Display(Name = "Character Id")]
        public string CharacterId { get; set; }


        [Required, Display(Name = "Assigned Role")]
        public string RoleName { get; set; }


        [Display(Name = "Character")]
        public string CharacterName { get; set; }


        [Display(Name = "Episode Id")]
        public string EpisodeId { get; set; }


        [Display(Name = "Episode Title")]
        public string Name { get; set; }


        [Display(Name = "Date Assigned")]
        public DateTime Created { get; set; }
    }


    public class EpisodeRoleRequestBody
    {
        [Required, Display(Name = "Character Id")]
        public string CharacterId { get; set; }


        [Required, Display(Name = "Assigned Role")]
        public string RoleName { get; set; }
    }
}
