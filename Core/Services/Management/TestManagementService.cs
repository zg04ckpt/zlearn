using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Management;
using Core.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Management
{
    public class TestManagementService : ITestManagementService
    {
        private readonly ITestRepository _testRepository;

        public TestManagementService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<APIResult<PaginatedResult<TestInfoDTO>>> GetAllTest(int pageSize, int pageIndex, List<ExpressionFilter> filters)
        {
            var tests = await _testRepository.GetPaginatedData(pageSize, pageIndex, filters);
            return new APISuccessResult<PaginatedResult<TestInfoDTO>>(new PaginatedResult<TestInfoDTO>
            {
                Total = tests.Total,
                Data = tests.Data.Select(e => TestMapper.MapToInfo(e))
            });
        }
    }
}
