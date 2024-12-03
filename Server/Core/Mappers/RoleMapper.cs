using Core.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
    public class RoleMapper
    {
        protected RoleDTO Map(AppRole role)
        {
            return new RoleDTO
            {
                Id = role.Id.ToString(),
                Name = role.Name,
                Description = role.Description
            };
        }

    }
}
