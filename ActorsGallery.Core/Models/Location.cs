using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActorsGallery.Core.Models
{
    public class Location
    {
        [Required, Column("LocationId")]
        public long Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public double Latitude { get; set; }


        [Required]
        public double Longitude { get; set; }


        [Required]
        public DateTime Created { get; set; }
    }
}
