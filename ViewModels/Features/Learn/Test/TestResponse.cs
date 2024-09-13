using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Features.Learn.Test.Question;

namespace ViewModels.Features.Learn.Test
{
    public class TestResponse
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<QuestionResponse> Questions { get; set; }
    }
}
