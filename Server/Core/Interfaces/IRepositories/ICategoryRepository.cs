using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category,  Guid>
    {
    }
}
