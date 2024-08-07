using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Web.Http.Results;
using Utilities.Exceptions;
using ViewModels.Common;

namespace BE.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleException(Exception ex)
        {
            var error = new
            {
                status = "error",
                message = ex.Message
            };

            if(ex is BadRequestException)
                return BadRequest(error);

            if (ex is NotFoundException)
                return NotFound(error);

            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}
