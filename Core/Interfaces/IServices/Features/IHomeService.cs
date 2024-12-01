using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Features
{
    public interface IHomeService
    {
        Task<APIResult<List<TestItemDTO>>> GetTopTests(int amount);
        Task<APIResult<List<TestItemDTO>>> GetRandomTests(int amount);
        Task<APIResult<List<UserInfoDTO>>> GetTopUser(int amount);
    }
}
