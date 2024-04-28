using Application.Practice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionServices _services;

        public QuestionsController(IQuestionServices services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByQuestionSetId(string id)
        {
            var result = await _services.GetByQuestionSetId(id);
            if(result.Code == System.Net.HttpStatusCode.InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }
    }
}
