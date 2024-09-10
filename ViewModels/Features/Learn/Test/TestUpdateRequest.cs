using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Features.Learn.Test.Question;

namespace ViewModels.Features.Learn.Test
{
    public class TestUpdateRequest
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public bool IsPrivate { get; set; }
        public List<QuestionUpdateRequest> Questions { get; set; }
    }
}
