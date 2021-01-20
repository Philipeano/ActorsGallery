using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
    [Display(Name = "Character")]
    public class CharacterDTO
    {
        [Display(Name = "Character Id")]
        public long Id { get; set; }


        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        public string Status { get; set; }


        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; } 


        [Required]
        public string Gender { get; set; }


        [Display(Name = "Location Id")]
        public string LocationId { get; set; } 


        [Display(Name = "Location")]
        public string LocationName { get; set; } 


        [Display(Name = "Created On")]
        public DateTime Created { get; set; }
    }


    public class CharacterRequestBody
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        public string Status { get; set; }


        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }


        [Required]
        public string Gender { get; set; }


        [Required, Display(Name = "Location Id")]
        public string LocationId { get; set; }
    }
}
