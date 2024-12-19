using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class LogDTO
    {
        public string Time { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public string? Detail { get; set; }
    }

    public class FileStreamDTO
    {
        public MemoryStream Stream { get; set; }
        public string Mime { get; set; }
        public string FileName { get; set; }
    }

}
