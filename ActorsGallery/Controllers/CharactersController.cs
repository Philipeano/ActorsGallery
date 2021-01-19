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
    public class CharactersController : ControllerBase
    {

        private readonly ICharacterData characterData;
        private readonly Formatter formatter;

        public CharactersController(ICharacterData charData)
        {
            characterData = charData;
            formatter = new Formatter();
        }


        // GET: api/characters
#pragma warning disable CS1570 // XML comment has badly formed XML
        /// <summary>
        /// Fetch all characters, with optional sorting and/or filtering
        /// </summary>
        /// <param name="filter_by">The property to filter results by. It must be assigned any of <c>gender</c>, <c>status</c>, <c>location</c> or <c>none</c>.</param>
        /// <param name="filter_val">The corresponding value of the <c>filter_by</c> parameter, which will be used for fetching the results.</param>
        /// <param name="sort_by">The property to sort results by. It must be assigned any of <c>firstname</c>, <c>lastname</c>, <c>gender</c> or <c>default</c>.</param>
        /// <param name="sort_dir">The sort direction to be applied. It must be assigned any of <c>asc</c>, <c>desc</c>, <c>ascending</c>, <c>descending</c> or <c>default</c>.</param>
        /// <example>
        ///     <code>GET: api/characters?filter_by=gender&filter_val=female&sort_by=lastname&sort_dir=asc</code>
        /// </example>
        /// <returns>A JSON object whose <c>Payload</c> property contains a list of <c>Character</c> objects, with the specified sorting or filtering applied.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
#pragma warning restore CS1570 // XML comment has badly formed XML
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/characters")]
        public ActionResult Get([FromQuery(Name = "filter_by")] string filter_by = "none",
                                [FromQuery(Name = "filter_val")] string filter_val = "none",
                                [FromQuery(Name = "sort_by")] string sort_by = "default",
                                [FromQuery(Name = "sort_dir")] string sort_dir = "default")
        {
            List<CharacterDTO> characters = characterData.GetCharacters(filter_by, filter_val, sort_by, sort_dir);
            if (characters != null)
            {
                return Ok(formatter.Render("Characters retrieved successfully.", characters));
            }
            else
            {
                return BadRequest(formatter.Render("No matching records found. This may be due to errors in your query."));
            }
        }


        // POST: api/characters
        /// <summary>
        /// Create a new character with the properties and values supplied in <c>characterObj</c>.  
        /// </summary>
        /// <param name="characterObj">A JSON object containing <c>FirstName</c>, <c>LastName</c>, <c>Status</c>, <c>StateOfOrigin</c>, <c>Gender</c> and <c>LocationId</c> properties.</param>
        /// <returns>A JSON object whose <c>Payload</c> property contains the newly created <c>Character</c> object, with missing properties included.</returns>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("api/characters")]
        public ActionResult Post([FromBody] CharacterDTO characterObj)
        {
            CharacterDTO newCharacter = characterData.CreateCharacter(characterObj, out string message);

            if (newCharacter != null)
            {
                return Created("", formatter.Render("Character created successfully.", newCharacter));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // PUT: api/characters/id
        /// <summary>
        /// Update an existing character with the properties and values supplied in <c>characterObj</c>.  
        /// </summary>
        /// <param name="id">The <c>id</c> of the character to be updated.</param>
        /// <param name="characterObj">A JSON object containing <c>FirstName</c>, <c>LastName</c>, <c>Status</c>, <c>StateOfOrigin</c>, <c>Gender</c> and <c>LocationId</c> properties.</param>
        /// <returns>A JSON object whose <c>Payload</c> property contains the updated <c>Character</c> object, with missing properties included.</returns>
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        ///// <response code="404">Not found! The specified resource does not exist.</response>
        ///// <response code="200">Success! Operation completed successfully</response> 
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBody))]
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] string id, [FromBody] CharacterDTO characterObj)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }


        // DELETE: api/characters/id
        /// <summary>
        /// Delete a character with the specified <c>id</c>   
        /// </summary>
        /// <param name="id">The <c>id</c> of the character to be deleted.</param>
        /// <returns>A JSON object whose <c>Payload</c> property has no value.</returns>
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        ///// <response code="404">Not found! The specified resource does not exist.</response>
        ///// <response code="200">Success! Operation completed successfully</response> 
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseBody))]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }
    }
}
