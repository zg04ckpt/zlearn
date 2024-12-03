using Core.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
    public class CommentMapper
    {
        public static Comment MapFromCreate(CreateCommentDTO dto)
        {
            return new Comment
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                CreatedAt = DateTime.Now,
                Likes = 0,
                ParentId = dto.ParentId != null? Guid.Parse(dto.ParentId) : null,
                TestId = Guid.Parse(dto.TestId)
            };
        }

        public static CommentDTO MapToDTO(Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id.ToString().ToLower(),
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                Likes = comment.Likes,
                ParentId = comment.ParentId?.ToString().ToLower(),
                UserId = comment.UserId.ToString().ToLower(),
                UserName = comment.User.UserName,
                UserAvatar = comment.User.ImageUrl
            };
        }
    }
}
