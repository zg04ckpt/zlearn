using API.Authorization;
using Core.DTOs;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public FilesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [Authorize("User")]
        [HttpPost("images/save")]
        public async Task<IActionResult> CreateImage([FromForm] IFormFile image)
        {
            return Ok(await _imageService.SaveImage(User, image));
        }


        [Authorize("User")]
        [HttpPost("images/update")]
        public async Task<IActionResult> UpdateImage([FromForm] IFormCollection data)
        {
            return Ok(await _imageService.UpdateImage(User, data));
        }
    
    }
}
