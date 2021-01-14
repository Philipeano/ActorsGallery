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
        public long EpisodeId { get; set; }


        [Required]
        public DateTime Created { get; set; }


        [Required]
        public long CommenterName { get; set; }


        public Episode Episode { get; set; }
    }
}
