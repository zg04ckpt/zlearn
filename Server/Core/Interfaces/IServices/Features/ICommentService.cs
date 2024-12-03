using Core.Common;
using Core.DTOs;
using System.Security.Claims;

namespace Core.Interfaces.IServices.Features
{
    public interface ICommentService
    {
        Task<APIResult<List<CommentDTO>>> GetCommentsOfTest(string testId);
        Task<APIResult> LikeComment(string commentId);
        Task<APIResult> CreateComment(ClaimsPrincipal claimsPrincipal, CreateCommentDTO dto);
        Task<APIResult> RemoveComment(ClaimsPrincipal claimsPrincipal, string commentId);
    }
}
