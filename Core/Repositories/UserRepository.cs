using Core.Interfaces.IRepositories;
using Data;
using Data.Entities;

namespace Core.Repositories
{
    public class UserRepository : BaseRepository<AppUser, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
