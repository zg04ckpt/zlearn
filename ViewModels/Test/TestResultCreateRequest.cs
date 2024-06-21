using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.QuestionSet;

namespace ViewModels.Test
{
    public class TestResultCreateRequest
    {
        public decimal Score { get; set; }
        public int CorrectsCount { get; set; }
        public TestTime UsedTime { get; set; }
        public DateTime StartTime { get; set; }
        public string UserInfo { get; set; } //temporary
        //----------------------------------------------
        public Guid QuestionSetId { get; set; }
    }
}
