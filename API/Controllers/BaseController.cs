using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using Utilities.Exceptions;

namespace BE.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected string GetUserIdFromClaimPrincipal()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value ?? null;
        }

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

            if (ex is ForbiddenException)
                return Forbid();

            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}
