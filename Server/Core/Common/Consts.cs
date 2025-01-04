using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class Consts
    {
        public const string ADMIN_ROLE = "Admin";
        public const string USER_ROLE = "UserConfig";
        public const string JWT_SECTION = "JwtSettings";
        public static class SettingKeys
        {
            public const string JWT_AUDIENCE = "JwtSettings:Audience";
        }
        public const string SECRET_KEY = "SECRET_KEY";
        public const string SYSTEM_EMAIL = "SYSTEM_EMAIL";
        public const string SYSTEM_EMAIL_PASS = "SYSTEM_EMAIL_PASS";
    }
}
