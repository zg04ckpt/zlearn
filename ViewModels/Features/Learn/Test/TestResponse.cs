using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class TestResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string ImageUrl { get; set; }
        public int QuestionCount { get; set; }
        public int AttemptCount { get; set; } 
        public int TestTime { get; set; }
    }
}
