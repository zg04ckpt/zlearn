using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.CommonEntities
{
    public class UploadedFile : BaseEntity
    {
        public Guid UploadedBy { get; set; }
        public Guid OwnedBy { get; set; }
        public string Path { get; set; }
        public FileType Type { get; set; }
    }
}
