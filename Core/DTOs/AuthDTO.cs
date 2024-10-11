using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }

    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username không được bỏ trống")]
        [StringLength(16, MinimumLength = 2, ErrorMessage = "Độ dài tên người dùng phải từ 2 đến 16 kí tự")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Tên người dùng không chứa kí tự đặc biệt (kể cả có dấu)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Độ dài mật khẩu phải từ 8 đến 16 kí tự")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.@!#*]).+$", ErrorMessage = "Mật khẩu phải chứa kí tự in thường, in hoa, chữ số và kí tự đặc biệt .@!#*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mật khẩu xác nhận không được bỏ trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }

    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
