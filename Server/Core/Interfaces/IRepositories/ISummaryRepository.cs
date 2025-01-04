using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface ISummaryRepository
    {
        Task SaveToDatabase(Summary summary);
        Task<List<Summary>> GetByRange(DateTime start, DateTime end);
    }
}
