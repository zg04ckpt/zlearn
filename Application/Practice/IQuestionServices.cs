using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;

namespace Application.Practice
{
    public interface IQuestionServices
    {
        Task<ApiResult> GetByQuestionSetId(string id);
    }
}
