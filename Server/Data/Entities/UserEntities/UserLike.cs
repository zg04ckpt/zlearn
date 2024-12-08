using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.UserEntities
{
    public class UserLike
    {
        public Guid LikedUserId { get; set; }
        public Guid UserId { get; set; }
    }
}
