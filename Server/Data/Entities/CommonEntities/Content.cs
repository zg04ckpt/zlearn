using Data.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.CommonEntities
{
    public class Content : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public Guid CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string? ImageUrl { get; set; }

        //Rela
        public Category Category { get; set; }
        public AppUser Author { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
