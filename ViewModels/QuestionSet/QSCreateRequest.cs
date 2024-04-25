using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Question;

namespace ViewModels.QuestionSet
{
    public class QSCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string ImageUrl { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
