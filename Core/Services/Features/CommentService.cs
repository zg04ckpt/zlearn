﻿using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Core.Services.Features
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITestRepository _testRepository;
        private readonly ISummaryService _summaryService;

        public CommentService(ICommentRepository commentRepository, ITestRepository testRepository, ISummaryService summaryService)
        {
            _commentRepository = commentRepository;
            _testRepository = testRepository;
            _summaryService = summaryService;
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
            comment.Content = Regex.Replace(comment.Content, @"\s+", " ");
            comment.UserId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            _commentRepository.Create(comment);
            if(await _commentRepository.SaveChanges())
            {
                // summary
                await _summaryService.IncreaseCommentCount();
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
            var isOwner = await _testRepository.IsExist(e => e.AuthorId.Equals(userId) && e.Id.Equals(comment.TestId));

            // Only test owner and admin can delete this comment
            if (!isOwner && !claimsPrincipal.IsInRole("Admin"))
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
