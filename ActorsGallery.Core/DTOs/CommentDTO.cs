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
        public string EpisodeId { get; set; }


        [Display(Name = "Episode Title")]
        public string EpisodeName { get; set; }
    }


    public class CommentRequestBody
    {
        [Required, Display(Name = "Commenter's Name")]
        public string CommenterName { get; set; }


        [Required, Display(Name = "Comment"), MaxLength(250)]
        public string CommentText { get; set; }
    }
}
