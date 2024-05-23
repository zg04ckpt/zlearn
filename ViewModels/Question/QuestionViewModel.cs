using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Question
{
    public class QuestionViewModel
    {
        public int Order { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
        public bool Mark { get; set; }
    }
}
