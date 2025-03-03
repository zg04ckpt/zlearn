﻿using Data.Entities.DocumentEntities;
using Data.Entities.PostEnttities;
using Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.CommonEntities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public Guid? ParentId { get; set; }
        public string Link { get; set; }

        //Rela
        public Category Parent { get; set; }
        public List<Category> Children { get; set; }
        public List<Test> Tests { get; set; }
        public List<Post> Posts { get; set; }
        public List<Document> Documents { get; set; }
    }
}
