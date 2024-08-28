using System;

namespace Data.Entities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public int Correct { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int UsedTime { get; set; }
        public Guid TestId { get; set; }
        public string TestName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
