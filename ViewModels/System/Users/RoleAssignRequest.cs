using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public List<RoleSelectModel> Roles { get; set; }
    }
    public class RoleSelectModel
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
