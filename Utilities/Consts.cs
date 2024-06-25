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

        public class EnvKey
        {
            public const string SECRET_KEY = "secret-key";
            public const string SYSTEM_EMAIL = "SYSTEM_EMAIL";
            public const string SYSTEM_EMAIL_PASS = "SYSTEM_EMAIL_PASS";
        }
    }
}
