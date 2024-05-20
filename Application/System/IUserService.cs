using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
{   
    public interface IUserService
    {
        Task<ApiResult> Authenticate(LoginRequest request);
    }
}
