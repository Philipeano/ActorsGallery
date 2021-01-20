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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentData commentData;
        private readonly Validator validator = new Validator();
        private readonly Formatter formatter;

        public CommentsController(ICommentData commentService)
        {
            commentData = commentService;
            formatter = new Formatter();
        }


        // GET: api/comments
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/comments")]
        public ActionResult Get()
        {
            List<CommentDTO> comments = commentData.GetAllComments();
            return Ok(formatter.Render("Comments retrieved successfully.", comments));
        }


        // GET: api/comments?episode_id={episodeId}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBody))]
        [HttpGet("api/comments?episode_id={episodeId}")]
        public ActionResult Get([FromQuery(Name = "episode_id")] string episode_id)
        {
            if (validator.ValidateEpisodeId(episode_id, out string message))
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
        [HttpPut("api/comments/{id}")]
        public ActionResult UpdateComment([FromRoute] string id, [FromBody] CommentDTO commentObj)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }


        // DELETE: api/comments/{id}
        [HttpDelete("api/comments/{id}")]
        public ActionResult DeleteComment([FromRoute] string id)
        {
            return BadRequest(formatter
                .Render("Sorry, this operation is currently not supported. \nIt will be supported in a future version of this API."));
        }
    }
}
