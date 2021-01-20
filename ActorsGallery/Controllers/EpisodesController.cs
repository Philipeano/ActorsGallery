using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;

namespace ActorsGallery.Controllers
{
    [Route("api/episodes")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodeData episodeData;
        private readonly Validator validator;
        private readonly Formatter formatter;

        public EpisodesController(IEpisodeData episodeService, IFetcher fetcherService)
        {
            episodeData = episodeService;
            validator = new Validator(fetcherService);
            formatter = new Formatter();
        }


        // This custom HTTP handler helps to produce 403Forbidden responses where the built-in handler is inadequate
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


        // GET: api/episodes
        /// <summary>
        /// Fetch all episodes, optionally queried with a featured character's id or name. The episodes are sorted in ascending order of 'releaseDate'.
        /// </summary>
        /// <param name="q">The search expression to filter results by. It must be in the format 'char_id:{value}' or 'char_name:{value}'.</param>
        /// <example>
        ///     <code>GET:  /api/episodes?q=char_id:5</code>
        ///     <code>GET:  /api/episodes?q=char_name:jackson</code>
        /// </example>
        /// <returns>A JSON object whose 'Payload' property contains a list of 'Episode' objects matching the query, sorted in ascending order of 'releaseDate'.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]       
        [HttpGet]
        public ActionResult Get([FromQuery(Name = "q")] string q = "default")
        {
            List<EpisodeDTO> episodes;

            if (!Request.Query.ContainsKey("q") || q == "default")
            {
                episodes = episodeData.GetAllEpisodes();
                return Ok(formatter.Render("Episodes retrieved successfully.", episodes));
            }
            else if (validator.IsValidQuery(q.Replace("%3A",":"), out string[] parts, out string message))
            {
                switch (parts[0])
                {
                    case "char_id":
                        episodes = episodeData.SearchByCharacter("id", parts[1]);
                        break;
                    default:
                        episodes = episodeData.SearchByCharacter("name", parts[1]);
                        break;
                }
                return Ok(formatter.Render("Episodes retrieved successfully.", episodes));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // POST: api/episodes
        /// <summary>
        /// Create a new episode with the properties and values supplied in the request body.  
        /// </summary>
        /// <param name="requestBody">A JSON object containing 'name', 'episodeCode' and 'releaseDate' properties.</param>
        /// <returns>A JSON object whose 'Payload' property contains the newly created 'Episode' object, with missing properties included.</returns>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost]
        public ActionResult PostEpisode([FromBody] EpisodeDTO requestBody)
        {
            EpisodeDTO newEpisode = episodeData.CreateEpisode(requestBody, out string message);

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
        /// <summary>
        /// Assign a character a role in an episode, using the character id and role name supplied in the request body.  
        /// </summary>
        /// <param name="id">The 'id' of the episode to assign the character a role in.</param>
        /// <param name="requestBody">A JSON object containing 'characterId', 'roleName' properties.</param>
        /// <returns>A JSON object whose 'Payload' property contains the newly created 'Episode' object, with missing properties included.</returns>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="409">Conflict! Duplicate role assignments are not allowed.</response>
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("{id}/characters")]
        public ActionResult PostCharacter([FromRoute] string id, [FromBody] EpisodeCharacterDTO requestBody)
        {
            EpisodeCharacterDTO newRole = episodeData.AddCharacter(id, requestBody, out string message);

            if (newRole != null)
            {
                return Created("", formatter.Render("Successfully assigned the character a role in the episode.", newRole));
            }
            else if (message.Contains("already has a role"))
            {
                return Conflict(formatter.Render(message));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // POST: api/episodes/{id}/comments
        /// <summary>
        /// Post a comment targeting an episode, using the values supplied in the request body, along with the commenter's public IP address.  
        /// </summary>
        /// <param name="id">The 'id' of the episode to post the comment to.</param>
        /// <param name="requestBody">A JSON object containing 'commentText', 'commenterName' properties.</param>
        /// <returns>A JSON object whose 'Payload' property contains the newly created 'Episode' object, with missing properties included.</returns>
        /// <response code="403">Forbidden! Unknown IP address.</response>
        /// <response code="201">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseBody))]
        [HttpPost("{id}/comments")]
        public ActionResult PostComment([FromRoute] string id, [FromBody] CommentDTO requestBody)
        {
            string userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(); ;

            if (userIpAddress == null || userIpAddress == string.Empty)
            {
                return Forbidden(formatter.Render("Unable to proceed. Your public IP address could not be determined."));
            }

            CommentDTO newComment = episodeData.AddComment(id, requestBody, userIpAddress, out string message);

            if (newComment != null)
            {
                return Created("", formatter.Render("Successfully posted a comment for the episode.", newComment));
            }
            else
            {
                return BadRequest(formatter.Render(message));
            }
        }


        // PUT: api/episodes/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpPut("{id}")]
        public ActionResult UpdateEpisode([FromRoute] string id, [FromBody] EpisodeDTO requestBody)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }


        // DELETE: api/episodes/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteEpisode([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }
    }
}
