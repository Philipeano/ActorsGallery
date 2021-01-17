using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
    public class EpisodeDTO
    {
        [Display(Name = "Episode Id")]
        public long Id { get; set; }


        [Required, Display(Name = "Episode Title")]
        public string Name { get; set; }


        [Required, Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }


        [Required, Display(Name = "Episode Code")]
        public string EpisodeCode { get; set; }


        [Display(Name = "Created On")]
        public DateTime Created { get; set; }
    }
}
