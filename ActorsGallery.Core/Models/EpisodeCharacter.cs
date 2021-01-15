using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.Models
{
    public class EpisodeCharacter
    {
        [Required]
        public long Id { get; set; }


        [Required]
        public long EpisodeId { get; set; }


        [Required]
        public long CharacterId { get; set; }


        [Required]
        public string RoleName { get; set; }


        [Required]
        public DateTime Created { get; set; }


        public Episode Episode { get; set; }


        public Character Character { get; set; }
    }
}