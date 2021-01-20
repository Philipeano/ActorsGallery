using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActorsGallery.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationData locationData;
        private readonly Formatter formatter;

        public LocationsController(ILocationData locationService)
        {
            locationData = locationService;
            formatter = new Formatter();
        }


        // GET: api/locations
        /// <summary>
        /// Fetch all locations.
        /// </summary>
        /// <returns>A JSON object whose 'Payload' property contains a list of 'Location' objects.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet]
        public ActionResult Get()
        {
            List<LocationDTO> locations = locationData.GetAllLocations();
            return Ok(formatter.Render("Locations retrieved successfully.", locations));
        }


        // POST: api/locations
        /// <summary>
        /// Create a new location with the properties and values supplied in the request body.  
        /// </summary>
        /// <param name="requestBody">A JSON object containing 'name', 'latitude' and 'longitude' properties.</param>
        /// <returns>A JSON object whose 'Payload' property contains the newly created 'Episode' object, with missing properties included.</returns>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost]
        public ActionResult Post([FromBody] LocationDTO requestBody)
        {
            LocationDTO newLocation = locationData.CreateLocation(requestBody, out string message);

            if (newLocation != null)
            {
                return Created("", formatter.Render("Location created successfully.", newLocation));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // PUT: api/locations/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpPut("{id}")]
        public ActionResult UpdateLocation([FromRoute] string id, [FromBody] LocationDTO requestBody)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }


        // DELETE: api/locations/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteLocation([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }
    }
}
