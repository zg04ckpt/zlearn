using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface ICommentRepository : IBaseRepository<Comment, Guid>
    {
        Task<List<Comment>> GetFullInfomationById(string testId);
        Task LikeComment(string commentId);
    }
}
