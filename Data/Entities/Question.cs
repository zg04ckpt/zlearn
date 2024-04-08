﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
        public int Score { get; set; }
        public bool Mark { get; set; }
        public Guid QuestionSetId { get; set; }
        public QuestionSet QuestionSet { get; set; }
    }
}
