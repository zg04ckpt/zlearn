using API.Authorization;
using Core.DTOs;
using Core.Interfaces.IServices.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{testId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsOfTest(string testId)
        {
            return Ok(await _commentService.GetCommentsOfTest(testId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO dto)
        {
            return Ok(await _commentService.CreateComment(User, dto));
        }

        [HttpDelete("{commentId}")]
        [Authorize]
        public async Task<IActionResult> RemoveComment(string commentId)
        {
            return Ok(await _commentService.RemoveComment(User, commentId));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> LikeComment([FromRoute] string id)
        {
            return Ok(await _commentService.LikeComment(id));
        }
    }
}
