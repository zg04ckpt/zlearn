using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Users;

namespace Application.System.Users
{
    public interface IUserService
    {
        Task<UserDetailModel> GetUserDetail(string id);
        Task UpdateUserDetail(string id, UserDetailModel request);
    }
}
