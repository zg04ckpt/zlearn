using Data.Entities.PostEnttities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.PostEntities
{
    public class PostImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public Guid PostId { get; set; }

        //Rela
        public Post Post { get; set; }
    }
}
