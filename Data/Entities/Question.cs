using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
        public int Score { get; set; } = 1;
        public bool Mark { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
