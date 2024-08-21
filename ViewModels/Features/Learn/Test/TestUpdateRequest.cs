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
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public bool Mark { get; set; }
        public List<QuestionCreateRequest> Questions { get; set; }
        public int TestTime { get; set; }
    }
}
