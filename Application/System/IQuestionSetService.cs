using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZG04WEB.ViewModels.Common;
using ZG04WEB.ViewModels.System;

namespace ZG04WEB.Application.System
{
    public interface IQuestionSetService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> GetById(string id);
        Task<ApiResult> Create(QuestionSetRequest request);
        Task<ApiResult> Update(string id, QuestionSetRequest request);
        Task<ApiResult> Delete(string id);
    }
}
