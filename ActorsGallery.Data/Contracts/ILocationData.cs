using ActorsGallery.Core.DTOs;
using System.Collections.Generic;

namespace ActorsGallery.Data.Contracts
{
    public interface ILocationData
    {
        // Relevant CRUD operations
        public List<LocationDTO> GetAllLocations();


        public LocationDTO CreateLocation(LocationRequestBody input, out string responseMsg);


        public LocationDTO UpdateLocation(string locationId, LocationRequestBody input, out string responseMsg);


        public void DeleteLocation(string locationId, out string responseMsg);
    }
}
