using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class MarkTestRequest
    {
        public List<QuestionAnswer> Answers { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TestId { get; set; }
        public string TestName { get; set; }
    }

    public class QuestionAnswer
    {
        public string Id { get; set; }
        public int Selected { get; set; }
    }
}
