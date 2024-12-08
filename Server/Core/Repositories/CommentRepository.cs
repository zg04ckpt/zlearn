using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.CommonEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class CommentRepository : BaseRepository<Comment, Guid>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Comment>> GetFullInfomationById(string testId)
        {
            return await _context.Comments
                .Include(x => x.User)
                .Where(x => x.TestId.ToString().Equals(testId))
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task LikeComment(string commentId)
        {
            int rowAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"UPDATE Comments SET Likes = Likes + 1 WHERE Id = {commentId}");

            if (rowAffected == 0)
            {
                throw new ErrorException("Đã có lỗi xảy ra");
            }
        }
    }
}
