using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateRoleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoleAssignmentDTO
    {
        public List<RoleSelectionDTO> Roles { get; set; }
    }

    public class RoleSelectionDTO
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
