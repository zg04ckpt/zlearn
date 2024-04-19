using  Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  ViewModels.System
{
    public class QuestionCreateRequest
    {
        public string Content { get; set; }
        public string Image { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
        public int Score { get; set; } = 1;
    }
}
