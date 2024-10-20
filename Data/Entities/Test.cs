using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
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
        public List<Question> Questions { get; set; }
        public List<SavedTest> UserInTests { get; set; }
        public Guid AuthorId { get; set; }
        public AppUser Author { get; set; }
    }
}
