using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActorsGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationData locationData;
        private readonly Validator validator = new Validator();
        private readonly Formatter formatter;

        public LocationsController(ILocationData locationService)
        {
            locationData = locationService;
            formatter = new Formatter();
        }


        // GET: api/locations
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/locations")]
        public ActionResult Get()
        {
            List<LocationDTO> locations = locationData.GetAllLocations();
            return Ok(formatter.Render("Locations retrieved successfully.", locations));
        }


        // POST: api/locations
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("api/locations")]
        public ActionResult Post([FromBody] LocationDTO locationObj)
        {
            LocationDTO newLocation = locationData.CreateLocation(locationObj, out string message);

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
        [HttpPut("api/locations/{id}")]
        public ActionResult UpdateLocation([FromRoute] string id, [FromBody] LocationDTO locationObj)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }


        // DELETE: api/locations/{id}
        [HttpDelete("api/locations/{id}")]
        public ActionResult DeleteLocation([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }

    }
}
