using Application.Practice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using ViewModels.Common;

namespace BE.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : BaseController
    {
        private readonly IQuestionServices _services;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IQuestionServices services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByQuestionSetId(string id)
        {
            var result = await _services.GetByQuestionSetId(id);
            return ApiResult(result);
        }
    }
}
