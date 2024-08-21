using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class SaveTestResultRequest
    {
        public decimal Score { get; set; }
        public int CorrectsCount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid UserId { get; set; } 
        public Guid TestId { get; set; }
        public string Detail { get; set; }
    }
}
