using ActorsGallery.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActorsGallery.Core.Models
{
    public class Comment
    {
        [Required, Column("CommentId")]
        public long Id { get; set; }


        [Required, Column("Comment"), MaxLength(250)]
        public string CommentText { get; set; }


        [Required, Column("IpAddressLocation")]
        public string IpAddress { get; set; }


        [Required]
        public StatusEnum Status { get; set; }


        public string StateOfOrigin { get; set; } // Nullable


        [Required]
        public GenderEnum Gender { get; set; }


        public long LocationId { get; set; } // Nullable


        [Required]
        public DateTime Created { get; set; }


        public virtual Location Location { get; set; } // Nullable


        public virtual List<Episode> Episodes { get; set; } // Nullable
    }
}
