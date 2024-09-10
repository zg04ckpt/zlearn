using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Consts
    {
        public const string CONNECTION_STRING = "DefaultConnection";
        public const string DEFAULT_USER_ROLE = "User";
        public const string DEFAULT_ADMIN_ROLE = "Admin";

        public class RoleName
        {
            public const string USER = "User";
            public const string ADMIN = "Admin";

        }

        public class AppSettingsKey
        {
            public const string ISSUER = "JwtSettings:Issuer";
            public const string AUDIENCE = "JwtSettings:Audience";
            public const string ACCESS_LIFE_TIME = "JwtSettings:AccessLifeTime";
            public const string REFRESH_LIFE_TIME = "JwtSettings:RefreshLifeTime";
        }

        public class EnvKey
        {
            public const string SECRET_KEY = "SECRET_KEY";
            public const string SYSTEM_EMAIL = "SYSTEM_EMAIL";
            public const string SYSTEM_EMAIL_PASS = "SYSTEM_EMAIL_PASS";
        }
    }
}
