using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class SavedTest
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public string MarkedAt { get; set; }
    }
}
