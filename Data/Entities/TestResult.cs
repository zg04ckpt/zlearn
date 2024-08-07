using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public decimal Score { get; set; }
        public int CorrectsCount { get; set; }
        public TimeSpan UsedTime { get; set; }
        public DateTime StartTime { get; set; }
        public string UserInfo { get; set; }
        public string Detail { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
