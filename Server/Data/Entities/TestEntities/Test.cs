using System;
using System.Collections.Generic;
using Data.Entities.CommonEntities;
using Data.Entities.UserEntities;

namespace Data.Entities.TestEntities
{
    public class Test : Content
    {
        public string Description { get; set; }
        public string Source { get; set; }
        public int Duration { get; set; }
        public int NumberOfAttempts { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool IsPrivate { get; set; }
        public List<Question> Questions { get; set; }
        public List<SavedTest> UserInTests { get; set; }
    }
}
