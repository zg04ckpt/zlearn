using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Enums;
using Data.Entities.TestEntities;
using Data.Entities.UserEntities;

namespace Data.Entities.CommonEntities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public CommentType type { get; set; }
        public Guid? ParentId { get; set; }
        public string TargetId { get; set; }
        public Guid UserId { get; set; }

        // relationships
        public AppUser User { get; set; }
    }
}
