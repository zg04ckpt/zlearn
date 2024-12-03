using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public string? ParentId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string? UserAvatar { get; set; }
    }

    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public string? ParentId { get; set; }
        public string TestId { get; set; }
    }
}
