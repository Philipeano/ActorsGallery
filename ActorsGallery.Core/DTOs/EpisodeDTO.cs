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
        public string ReleaseDate { get; set; }


        [Required, Display(Name = "Episode Code")]
        public string EpisodeCode { get; set; }


        [Display(Name = "Created On")]
        public DateTime Created { get; set; }


        [Display(Name = "Number of Comments")]
        public int NumOfComments { get; set; }


        [Display(Name = "Number of Characters")]
        public int NumOfCharacters { get; set; }
    }


    public class EpisodeRequestBody
    {
        [Required, Display(Name = "Episode Title")]
        public string Name { get; set; }


        [Required, Display(Name = "Release Date")]
        public string ReleaseDate { get; set; }


        [Required, Display(Name = "Episode Code")]
        public string EpisodeCode { get; set; }
    }
}
