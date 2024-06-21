using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.QuestionSet;

namespace Application.Practice
{
    public interface IQuestionSetService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> GetById(string id);
        Task<ApiResult> Create(QSCreateRequest request);
        Task<ApiResult> Update(string id, QSUpdateRequest request);
        Task<ApiResult> Delete(string id);
    }
}
