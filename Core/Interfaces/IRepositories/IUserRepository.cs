
using Data.Entities;

namespace Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<AppUser, Guid>
    {
    }
}
