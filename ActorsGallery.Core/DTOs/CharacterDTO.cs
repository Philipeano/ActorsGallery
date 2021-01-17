using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
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
        public string StateOfOrigin { get; set; } // Nullable


        [Required]
        public string Gender { get; set; }


        [Display(Name = "Location Id")]
        public string LocationId { get; set; } // Nullable


        [Display(Name = "Location")]
        public string LocationName { get; set; } // Nullable


        [Display(Name = "Created On")]
        public DateTime Created { get; set; }
    }
}
