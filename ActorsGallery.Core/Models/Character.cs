using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActorsGallery.Core.Models
{
    public class Character
    {
        [Required, Column("CharacterId")]
        public long Id { get; set; }


        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }


        [Required]
        public string Status { get; set; }


        [Required]
        public string StateOfOrigin { get; set; } // Nullable


        [Required]
        public string Gender { get; set; }


        public long LocationId { get; set; } // Nullable


        [Required]
        public DateTime Created { get; set; }


        public Location Location { get; set; } // Nullable


        public virtual List<EpisodeCharacter> EpisodesFeaturedIn { get; set; }
    }
}
