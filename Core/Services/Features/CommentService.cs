using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Core.Mappers;
using System.Security.Claims;

namespace Core.Services.Features
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<APIResult> CreateComment(ClaimsPrincipal claimsPrincipal, CreateCommentDTO dto)
        {
            var comment = CommentMapper.MapFromCreate(dto);
            comment.UserId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            _commentRepository.Create(comment);
            if(await _commentRepository.SaveChanges())
            {
                return new APISuccessResult("Đã gửi bình luận!");
            } 
            else
            {
                return new APIErrorResult("Bình luận thất bại!");
            }
        }

        public async Task<APIResult<List<CommentDTO>>> GetCommentsOfTest(string testId)
        {
            var comments = await _commentRepository.GetFullInfomationById(testId);
            return new APISuccessResult<List<CommentDTO>>(comments.Select(x => CommentMapper.MapToDTO(x)).ToList());
        }

        public async Task<APIResult> LikeComment(string commentId)
        {
            if(!await _commentRepository.IsExist(x => x.Id.ToString().Equals(commentId)))
            {
                throw new ErrorException("Bình luận không tồn tại");
            }

            await _commentRepository.LikeComment(commentId);
            return new APISuccessResult();
        }

        public async Task<APIResult> RemoveComment(ClaimsPrincipal claimsPrincipal, string commentId)
        {
            var userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            var comment = await _commentRepository.GetById(Guid.Parse(commentId));
            if(comment.UserId != userId)
            {
                throw new ForbiddenException();
            }
            _commentRepository.Delete(comment);
            if (await _commentRepository.SaveChanges())
            {
                return new APISuccessResult("Đã xóa bình luận!");
            }
            else
            {
                return new APIErrorResult("Xóa bình luận thất bại!");
            }
        }
    }
}
