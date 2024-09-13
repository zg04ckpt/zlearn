using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Features.Learn.Test.Question;

namespace ViewModels.Features.Learn.Test
{
    public class TestUpdateContent
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public bool IsPrivate { get; set; }
        public List<QuestionUpdateContent> Questions { get; set; }
    }
}
