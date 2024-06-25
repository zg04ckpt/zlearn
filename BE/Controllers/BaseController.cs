using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ViewModels.Common;

namespace BE.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult ApiResult(ApiResult result)
        {
            if (result.Code == HttpStatusCode.OK)
                return Ok(result);
            if (result.Code == HttpStatusCode.BadRequest)
                return BadRequest(result);
            if (result.Code == HttpStatusCode.NotFound)
                return NotFound(result);
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
