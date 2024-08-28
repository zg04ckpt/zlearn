using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Features.Learn.Test.Question;

namespace ViewModels.Features.Learn.Test
{
    public class CreateTestRequest
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public int Duration { get; set; }
        public List<QuestionCreateRequest> Questions { get; set; }
    }
}
