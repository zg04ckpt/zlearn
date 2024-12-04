using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Management
{
    public interface ITestManagementService
    {
        Task<APIResult<PaginatedResult<TestInfoDTO>>> GetAllTest(int pageSize, int pageIndex, List<ExpressionFilter> filters);
    }
}
