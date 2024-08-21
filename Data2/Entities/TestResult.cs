using System;

namespace Data.Entities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public int CorrectsCount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string? Detail { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
