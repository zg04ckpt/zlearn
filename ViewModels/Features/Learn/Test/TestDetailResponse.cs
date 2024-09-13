using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Features.Learn.Test
{
    public class TestDetailResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfAttempts { get; set; }
        public bool IsPrivate { get; set; }
    }
}
