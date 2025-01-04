using System;

namespace Data.Entities.TestEntities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public int Correct { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int UsedTime { get; set; }
        public Guid TestId { get; set; }
        public string TestName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
