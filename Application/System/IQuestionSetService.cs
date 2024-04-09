using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System;

namespace Application.System
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
