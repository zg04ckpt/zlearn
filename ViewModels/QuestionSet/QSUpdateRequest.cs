using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Question;

namespace ViewModels.QuestionSet
{
    public class QSUpdateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public bool Mark { get; set; }
        public List<QuestionRequest> Questions { get; set; }
    }
}
