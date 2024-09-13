using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class TestResultResponse
    {
        public int Total { get; set; }
        public double Score { get; set; }
        public int Correct { get; set; }
        public int Unselected { get; set; }
        public int UsedTime { get; set; }
        public List<int> Detail { get; set; } 
    }
}
