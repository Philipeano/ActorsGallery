using System;
using System.ComponentModel.DataAnnotations;

namespace ActorsGallery.Core.DTOs
{
    public class LocationDTO
    {
        [Display(Name = "Location Id")]
        public long Id { get; set; }


        [Required, Display(Name = "Location Name")]
        public string Name { get; set; }


        [Required]
        public string Latitude { get; set; }


        [Required]
        public string Longitude { get; set; }


        [Display(Name = "Created On")]
        public DateTime Created { get; set; }
    }




    public class LocationRequestBody
    {
        [Required, Display(Name = "Location Name")]
        public string Name { get; set; }


        [Required]
        public string Latitude { get; set; }


        [Required]
        public string Longitude { get; set; }
    }
}
