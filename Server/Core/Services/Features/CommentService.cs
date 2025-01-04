using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Data.Entities.TestEntities;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Services.Features
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITestRepository _testRepository;
        private readonly ISummaryService _summaryService;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, ITestRepository testRepository, ISummaryService summaryService, INotificationService notificationService, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _testRepository = testRepository;
            _summaryService = summaryService;
            _notificationService = notificationService;
            _userRepository = userRepository;
        }

        public async Task<APIResult> CreateComment(ClaimsPrincipal claimsPrincipal, CreateCommentDTO dto)
        {
            var words = Regex.Split(dto.Content, @"\s+");
            foreach(var word in words)
            {
                if (word.Length > 30)
                {
                    throw new ErrorException("Bình luận của bạn chứa từ > 40 kí tự");
                }
            }

            var comment = CommentMapper.MapFromCreate(dto);
            // Format comment
            comment.Content = Regex.Replace(comment.Content, @"\s+", " ");
            comment.UserId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            _commentRepository.Create(comment);
            if(await _commentRepository.SaveChanges())
            {
                // summary
                await _summaryService.IncreaseCommentCount();

                // create notification for owner
                var test = await _testRepository.GetById(Guid.Parse(dto.TestId));
                var commenterName = (await _userRepository.GetById(comment.UserId)).UserName;
                await _notificationService.CreateNewNotification(new CreateNotificationDTO
                {
                    Title = "Thông báo bình luận mới",
                    Message = $"{commenterName} đã bình luận về đề trắc nghiệm {test.Name}: {dto.Content}",
                    Type = Data.Entities.Enums.NotificationType.User,
                    UserId = test.AuthorId.ToString()
                });

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
            // sort by created time (DESC)
            comments.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            return new APISuccessResult<List<CommentDTO>>(comments.Select(x => CommentMapper.MapToDTO(x)).ToList());
        }

        public async Task<APIResult> LikeComment(string commentId)
        {
            if (!await _commentRepository.IsExist(x => x.Id.ToString().Equals(commentId)))
            {
                throw new ErrorException("Bình luận không tồn tại");
            }

            await _commentRepository.LikeComment(commentId);
            return new APISuccessResult();
        }

        public async Task<APIResult> RemoveComment(ClaimsPrincipal claimsPrincipal, string commentId)
        {
            var userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            var comment = await _commentRepository.GetById(Guid.Parse(commentId))
                ?? throw new ErrorException("Bình luận không tồn tại!");
            var isOwner = await _testRepository.IsExist(e => e.AuthorId.Equals(userId) && e.Id.ToString().Equals(comment.TargetId));

            // Only test owner and admin can delete this comment
            if (!isOwner && !claimsPrincipal.IsInRole("Admin"))
            {
                throw new ForbiddenException();
            }
            _commentRepository.Delete(comment);
            if (await _commentRepository.SaveChanges())
            {
                // Push notification for commnent owner
                var test = await _testRepository.GetById(Guid.Parse(comment.TargetId));
                await _notificationService.CreateNewNotification(new CreateNotificationDTO
                {
                    Title = "Bình luận bị xóa",
                    Message = $"Bình luận của bạn về đề trắc nghiệm {test.Name} đã bị {claimsPrincipal.FindFirstValue(ClaimTypes.Name)!} xóa!",
                    Type = Data.Entities.Enums.NotificationType.User,
                    UserId = comment.UserId.ToString()
                });

                return new APISuccessResult("Đã xóa bình luận!");
            }
            else
            {
                return new APIErrorResult("Xóa bình luận thất bại!");
            }
        }
    }
}
