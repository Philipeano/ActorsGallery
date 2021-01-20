using ActorsGallery.Core.DTOs;
using ActorsGallery.Core.Models;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using System;
using System.Collections.Generic;

namespace ActorsGallery.Data.MySqlDataService
{
    public class LocationData : ILocationData
    {
        private readonly ActorsGalleryContext context;
        private readonly IFetcher fetcher;
        private readonly Validator validator;

        public LocationData(ActorsGalleryContext dbContext, IFetcher dataFetcher)
        {
            context = dbContext;
            fetcher = dataFetcher;
            validator = new Validator(dataFetcher);
        }


        public List<LocationDTO> GetAllLocations()
        {
            // Fetch all locations 
            List<Location> locations = fetcher.FetchAllLocations();

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


        public LocationDTO CreateLocation(LocationRequestBody input, out string responseMsg)
        {
            if (!validator.ValidateLocationObj(input, out responseMsg))
            {
                return null;
            }
            else
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

                // Display new record using public-facing DTO, rather than internal data representation 
                return new LocationDTO
                {
                    Id = newLocation.Id,
                    Name = newLocation.Name,
                    Latitude = newLocation.Latitude.ToString(),
                    Longitude = newLocation.Longitude.ToString(),
                    Created = newLocation.Created
                };
            }
        }


        public LocationDTO UpdateLocation(string locationId, LocationRequestBody input, out string responseMsg)
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
