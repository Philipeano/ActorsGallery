using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
    public class CommentDTO
    {
        [Display(Name = "Comment Id")]
        public long Id { get; set; }


        [Required, Display(Name = "Comment"), MaxLength(250)]
        public string CommentText { get; set; }


        [Required, Display(Name = "Commenter's Name")]
        public string CommenterName { get; set; }


        [Display(Name = "Commenter's IP Address")]
        public string IpAddress { get; set; }


        [Display(Name = "Posted On")]
        public DateTime Created { get; set; }


        [Display(Name = "Episode Id")]
        public long EpisodeId { get; set; }


        [Display(Name = "Episode Title")]
        public long EpisodeName { get; set; }
    }
}
