using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class TestItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfAttempts { get; set; }
        public bool IsPrivate { get; set; }
    }
}
