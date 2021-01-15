using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActorsGallery.Core.Models
{
    public class Episode
    {
        [Required, Column("EpisodeId")]
        public long Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public DateTime ReleaseDate { get; set; }


        [Required]
        public string EpisodeCode { get; set; }


        [Required]
        public DateTime Created { get; set; }


        public virtual List<EpisodeCharacter> EpisodeCharacters { get; set; } 


        public virtual List<Comment> EpisodeComments { get; set; } 
    }
}