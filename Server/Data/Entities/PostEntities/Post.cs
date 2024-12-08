using Data.Entities.CommonEntities;
using Data.Entities.PostEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.PostEnttities
{
    public class Post : Content
    {
        public string Title { get; set; }
        public string Content { get; set; }

        //Rela
        public List<PostImage> Images { get; set; }
    }
}
