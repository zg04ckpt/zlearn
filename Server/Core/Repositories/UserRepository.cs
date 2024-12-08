using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class UserRepository : BaseRepository<AppUser, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<List<AppUser>> GetTopByLikes(int amount)
        {
            return await _context.AppUsers.AsNoTracking()
                .OrderByDescending(x => x.Likes)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<bool> IsLiked(string userId, string likedUserId)
        {
            return await _context.UserLikes.AsNoTracking()
                .AnyAsync(x => x.UserId == Guid.Parse(userId) && x.LikedUserId == Guid.Parse(likedUserId));
        }

        public async Task Like(string userId, string likedUserId)
        {
            _context.Database.ExecuteSqlRaw("UPDATE AppUsers SET Likes = Likes + 1 WHERE Id = {0}", likedUserId);
            var userLike = new UserLike
            {
                UserId = Guid.Parse(userId),
                LikedUserId = Guid.Parse(likedUserId),

            };
            _context.Set<UserLike>().Add(userLike);
            await _context.SaveChangesAsync();
        }
    }
}
