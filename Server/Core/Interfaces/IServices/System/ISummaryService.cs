using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.System
{
    public interface ISummaryService
    {
        APIResult<SummaryDTO> GetToday();
        Task<APIResult<SummaryDTO>> GetByRange(string start, string end);
        Task IncreaseAccessCount(string ip);
        Task IncreaseRequestCount();
        Task IncreaseTestCompletionCount();
        Task IncreaseUserCount();
        Task IncreaseCommentCount();
    }
}
