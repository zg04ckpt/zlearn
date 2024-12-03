using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("top-tests")]
        public async Task<IActionResult> GetTopTests(int amount)
        {
            var result = await _homeService.GetTopTests(amount);
            return Ok(result);
        }

        [HttpGet("random-tests")]
        public async Task<IActionResult> GetRandomTests(int amount)
        {
            var result = await _homeService.GetRandomTests(amount);
            return Ok(result);
        }

        [HttpGet("top-users")]
        public async Task<IActionResult> GetTopUsers(int amount)
        {
            var result = await _homeService.GetTopUser(amount);
            
            return Ok(result);
        }
    }
}
