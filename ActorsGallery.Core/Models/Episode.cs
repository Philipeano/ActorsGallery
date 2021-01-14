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


        public virtual List<Character> Characters { get; set; } // Nullable


        public virtual List<Comment> EpisodeComments { get; set; } // Nullable


        [Required]
        public DateTime Created { get; set; }
    }
}