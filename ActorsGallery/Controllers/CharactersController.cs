using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActorsGallery.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterData characterData;
        private readonly Formatter formatter;

        public CharactersController(ICharacterData characterService)
        {
            characterData = characterService;
            formatter = new Formatter();
        }


        // GET: api/characters
        /// <summary>
        /// Fetch all characters, with optional sorting and/or filtering
        /// </summary>
        /// <param name="filter_by">The property to filter results by. It must be assigned any of 'gender', 'status', 'location' or 'none'.</param>
        /// <param name="filter_val">The corresponding value of the 'filter_by' parameter, which will be used for fetching the results.</param>
        /// <param name="sort_by">The property to sort results by. It must be assigned any of 'firstname', 'lastname', 'gender' or 'default'.</param>
        /// <param name="sort_dir">The sort direction to be applied. It must be assigned any of 'asc', 'desc', 'ascending', 'descending' or 'default'.</param>
        /// <returns>A JSON object whose 'Payload' property contains a list of 'Character' objects, with the specified sorting or filtering applied.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet]
        public ActionResult Get([FromQuery(Name = "filter_by")] string filter_by = "none",
                                [FromQuery(Name = "filter_val")] string filter_val = "none",
                                [FromQuery(Name = "sort_by")] string sort_by = "default",
                                [FromQuery(Name = "sort_dir")] string sort_dir = "default")
        {

            List<CharacterDTO> characters;

            if (Request.Query.Count == 0)
            {
                characters = characterData.GetCharacters("none", "none", "default", "default");
            }
            else
            {
                characters = characterData.GetCharacters(filter_by, filter_val, sort_by, sort_dir);
            }

            if (characters != null)
            {
                return Ok(formatter.Render("Characters retrieved successfully.", characters));
            }
            else
            {
                return BadRequest(formatter.Render("Unable to retrieve any records. Please check your URL for errors and try again."));
            }
        }


        // POST: api/characters
        /// <summary>
        /// Create a new character with the properties and values supplied in the request body.  
        /// </summary>
        /// <param name="requestBody">A JSON object containing 'firstName', 'lastName', 'status', 'stateOfOrigin', 'gender' and 'locationId' properties.</param>
        /// <returns>A JSON object whose 'Payload' property contains the newly created 'Character' object, with missing properties included.</returns>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost]
        public ActionResult Post([FromBody] CharacterRequestBody requestBody)
        {
            CharacterDTO newCharacter = characterData.CreateCharacter(requestBody, out string message);

            if (newCharacter != null)
            {
                return Created("", formatter.Render("Character created successfully.", newCharacter));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // PUT: api/characters/{id}
        /// <summary>
        /// Update an existing character with the properties and values supplied in the request body.THIS FEATURE IS NOT SUPPORTED AT THE MOMENT.
        /// </summary>
        /// <param name = "id" > The 'id' of the character to be updated.</param>
        /// <param name="requestBody">A JSON object containing the character object to be updated.</param>
        /// <response code = "400" > Bad request! Check for any error, and try again.</response>
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] string id, [FromBody] CharacterRequestBody requestBody)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }


        // DELETE: api/characters/{id}
        /// <summary>
        /// Delete a character with the specified 'id'. THIS FEATURE IS NOT SUPPORTED AT THE MOMENT.   
        /// </summary>
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }
    }
}
