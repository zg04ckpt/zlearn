
using Data.Entities;

namespace Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<AppUser, Guid>
    {
        Task<bool> IsLiked(string userId, string likedUserId);
        Task Like(string userId, string likedUserId);
        Task<List<AppUser>> GetTopByLikes(int amount);
    }
}
