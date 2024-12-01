using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SummaryDTO
    {
        public int AccessCount { get; set; }
        public int TestCompletionCount { get; set; }
        public int CommentCount { get; set; }
        public int UserCount { get; set; }
    }
}
