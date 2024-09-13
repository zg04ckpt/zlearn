using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.System.Manage
{
    public class TestManagementModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public string AuthorName { get; set; }
        public int NumberOfAttempts { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool IsPrivate { get; set; }
        public string AuthorId { get; set; }
    }
}
