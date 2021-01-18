using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorsGallery.Data.MySqlDataService
{
    public class LocationData : ILocationData
    {
        private readonly ActorsGalleryContext context;


        public LocationData(ActorsGalleryContext dbContext)
        {
            context = dbContext;
        }


        private List<Location> FetchAllLocations()
        {
            return context.Locations
                 .ToList();
        }


        private Location FetchLocationById(string strLocationId)
        {
            if (!long.TryParse(strLocationId, out long valLocationId) || valLocationId < 1)
            {
                return null;
            }
            else
            {
                return context.Locations.Find(valLocationId);
            }
        }


        private bool ValidateLocationObj(LocationDTO input, out string errorMsg)
        {
            if (input == null)
            {
                errorMsg = "Input object cannot be null or empty.";
                return false;
            }

            else
            {
                errorMsg = string.Empty;

                if (input.Name == string.Empty)
                    errorMsg = "'Location Name' is required. \n";

                if (double.TryParse(input.Latitude, out double valLatitude))
                    errorMsg = $"{errorMsg}'Latitude' must be a valid double-precision number. \n";

                if (valLatitude < -90 || valLatitude > 90)
                    errorMsg = $"{errorMsg}'Latitude' must be in the range -90 to +90. \n";

                if (double.TryParse(input.Longitude, out double valLongitude))
                    errorMsg = $"{errorMsg}'Longitude' must be a valid double-precision number. \n";

                if (valLongitude < -180 || valLongitude > 180)
                    errorMsg = $"{errorMsg}'Longitude' must be in the range -180 to +180. \n";

                return errorMsg.Trim() == string.Empty;
            }
        }


        public List<LocationDTO> GetAllLocations()
        {
            // Fetch all available locations 
            List<Location> locations = FetchAllLocations();

            // Render results using public-facing DTOs, rather than internal data representation 
            List<LocationDTO> resultSet = new List<LocationDTO> { };
            foreach (var location in locations)
            {
                resultSet.Add(new LocationDTO
                {
                    Id = location.Id,
                    Name = location.Name,
                    Latitude = location.Latitude.ToString(),
                    Longitude = location.Longitude.ToString(),
                    Created = location.Created
                });
            }
            return resultSet;
        }


        public LocationDTO CreateLocation(LocationDTO input, out string responseMsg)
        {
            if (ValidateLocationObj(input, out responseMsg) == true)
            {
                // Validation checks passed. Create new Location record
                Location newLocation = new Location
                {
                    Name = input.Name,
                    Latitude = double.Parse(input.Latitude),
                    Longitude = double.Parse(input.Longitude),
                    Created = DateTime.UtcNow,
                };
                context.Locations.Add(newLocation);
                context.SaveChanges();

                return new LocationDTO
                {
                    Id = newLocation.Id,
                    Name = newLocation.Name,
                    Latitude = newLocation.Latitude.ToString(),
                    Longitude = newLocation.Longitude.ToString(),
                    Created = newLocation.Created
                };
            }
            else
            {
                return null;
            }
        }


        public LocationDTO UpdateLocation(string locationId, LocationDTO input, out string responseMsg)
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate locationId and input arguments
              - Retrieve matching location record, if any
              - Assign responseMsg a value, if necessary
              - Update the location record with the new values
              - Return updated location record
             */

            return new LocationDTO { };
        }


        public void DeleteLocation(string locationId, out string responseMsg)
        {
            responseMsg = string.Empty;

            /*
             TODOs:
              - Validate locationId
              - Delete matching location record, if any
              - Assign responseMsg a value, if necessary
             */
        }
    }
}
