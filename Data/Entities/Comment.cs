using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public Guid? ParentId { get; set; }

        // relationships
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
