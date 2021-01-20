using ActorsGallery.Core.DTOs;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ActorsGallery.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentData commentData;
        private readonly Validator validator;
        private readonly Formatter formatter;

        public CommentsController(ICommentData commentService, IFetcher fetcherService)
        {
            commentData = commentService;
            validator = new Validator(fetcherService);
            formatter = new Formatter();
        }


        // GET: api/comments
        /// <summary>
        /// Fetch all comments, optionally queried with an episode id. The comments are sorted in reverse chronological order by default.
        /// </summary>
        /// <param name="episode_id">An optional episode id to filter the comments by.</param>
        /// <example>
        ///     <code>GET:  /api/comments?episode_id=5</code>
        /// </example>
        /// <returns>A JSON object whose 'Payload' property contains a list of 'Comment' objects matching the query, sorted in reverse chronological order by default.</returns>
        /// <response code="200">Success! Operation completed successfully</response> 
        /// <response code="400">Bad request! Check for any error, and try again.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet]
        public ActionResult Get([FromQuery(Name = "episode_id")] string episode_id = "")
        {
            if (!Request.Query.ContainsKey("episode_id") || episode_id == "default")
            {
                List<CommentDTO> comments = commentData.GetAllComments();
                return Ok(formatter.Render("Comments retrieved successfully.", comments));
            }
            else if (validator.ValidateEpisodeId(episode_id, out string message))
            {
                List<CommentDTO> comments = commentData.GetEpisodeComments(episode_id);
                return Ok(formatter.Render("Comments retrieved successfully.", comments));
            }
            else
            {
                if (message.Contains("does not match"))
                    return NotFound(formatter.Render(message));
                else
                    return BadRequest(formatter.Render(message));
            }
        }


        // PUT: api/comments/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpPut("{id}")]
        public ActionResult UpdateComment([FromRoute] string id, [FromBody] CommentDTO requestBody)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }


        // DELETE: api/comments/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteComment([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. It will be supported in a future version of this API."));
        }
    }
}
