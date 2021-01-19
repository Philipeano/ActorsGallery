using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ActorsGallery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodeData episodeData;
        private readonly Validator validator = new Validator();
        private readonly Formatter formatter;

        public EpisodesController(IEpisodeData episodeService)
        {
            episodeData = episodeService;
            formatter = new Formatter();
        }


        // GET: api/episodes
        /// <summary>
        /// Fetch all episodes, sorted in ascending order of <c>releaseDate</c>.
        /// </summary>
        /// <returns>A JSON object whose <c>Payload</c> property contains a list of <c>Episode</c> objects, sorted in ascending order of <c>releaseDate</c>.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/episodes")]
        public ActionResult Get()
        {
            List<EpisodeDTO> episodes = episodeData.GetAllEpisodes();
            return Ok(formatter.Render("Episodes retrieved successfully.", episodes));
        }


        // GET: api/episodes?query={query_expression}
        /// <summary>
        /// Fetch all episodes featuring a particular character, sorted in ascending order of <c>releaseDate</c>.
        /// </summary>
        /// <param name="query">The search expression to filter results by. It must be in the format <c>char_id:{value}</c> or <c>char_name:{value}</c>.</param>
        /// <example>
        ///     <code>GET:  /api/episodes?query=char_id:5</code>
        ///     <code>GET:  /api/episodes?query=char_name:jackson</code>
        /// </example>
        /// <returns>A JSON object whose <c>Payload</c> property contains a list of <c>Episode</c> objects matching the query, sorted in ascending order of <c>releaseDate</c>.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="404">Not found! The specified character did not feature .</response>
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/episodes?query={query_expression}")]
        public ActionResult Get([FromQuery(Name = "query")] string query)
        {
            if (validator.IsValidQuery(query, out string[] parts, out string message))
            {
                List<EpisodeDTO> episodes;
                switch (parts[0])
                {
                    case "char_id":
                            episodes = episodeData.SearchByCharacter("id", parts[1]);
                            return Ok(formatter.Render("Episodes retrieved successfully.", episodes));

                    default:
                        episodes = episodeData.SearchByCharacter("name", parts[1]);
                        return Ok(formatter.Render("Episodes retrieved successfully.", episodes));
                }
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // POST: api/episodes
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("api/episodes")]
        public ActionResult PostEpisode([FromBody] EpisodeDTO episodeObj)
        {
            EpisodeDTO newEpisode = episodeData.CreateEpisode(episodeObj, out string message);

            if (newEpisode != null)
            {
                return Created("", formatter.Render("Episode created successfully.", newEpisode));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // POST: api/episodes/{id}/characters
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("api/episodes/{id}/characters")]
        public ActionResult PostCharacter([FromRoute] string id, [FromBody] EpisodeCharacterDTO roleObj)
        {
            EpisodeCharacterDTO newRole = episodeData.AddCharacter(id, roleObj, out string message);

            if (newRole != null)
            {
                return Created("", formatter.Render("Successfully assigned the character a role in the episode.", newRole));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // POST: api/episodes/{id}/comments
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("api/episodes/{id}/comments")]
        public ActionResult PostComment([FromRoute] string id, [FromBody] CommentDTO commentObj)
        {
            string userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(); ;

            if (userIpAddress == null || userIpAddress == string.Empty)
            {
                return Forbidden(formatter.Render("Unable to proceed. Your public IP address could not be determined."));
            }

            CommentDTO newComment = episodeData.AddComment(id, commentObj, userIpAddress, out string message);

            if (newComment != null)
            {
                return Created("", formatter.Render("Successfully posted a comment for the episode.", newComment));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }

        /// <summary>
        /// A custom HTTP handler for generating 403Forbidden responses where the built-in handlers are inadequate
        /// </summary>
        private ActionResult Forbidden(ResponseBody responseBody)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Response.StatusCode = 403;
            Response.ContentType = "application/json";
            _ = Response.WriteAsync(responseBody.ToString(), System.Text.Encoding.Default, token);
            source.Dispose();
            return null;
        }


        // PUT: api/episodes/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpPut("api/episodes/{id}")]
        public ActionResult UpdateEpisode([FromRoute] string id, [FromBody] EpisodeDTO episodeObj)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }


        // DELETE: api/episodes/{id}
        [HttpDelete("api/episodes/{id}")]
        public ActionResult DeleteEpisode([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }
    }
}
