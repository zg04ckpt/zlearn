using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System.Auth;

namespace Application.System.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task Register(RegisterRequest request, string origin);
        Task Logout();
        Task<Token> RefreshToken(Token token);
        Task EmailValidate(string userId, string token);
    }
}
