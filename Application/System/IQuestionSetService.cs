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
        Task<ApiResult> GetById(Guid id);
        Task<ApiResult> Create(QuestionSetCreateRequest request);
        Task<ApiResult> Update(Guid id, QuestionSetCreateRequest request);
        Task<ApiResult> Delete(Guid id);
    }
}
