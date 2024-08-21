using System;
using System.Collections.Generic;

namespace  Data.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string? ImageUrl { get; set; }
        public int Duration { get; set; }
        public List<Question> Questions { get; set; }
        public List<TestResult> TestResults { get; set; }
    }
}
