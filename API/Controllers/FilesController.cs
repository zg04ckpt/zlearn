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
        [HttpPost("images")]
        public async Task<IActionResult> CreateImage([FromForm] IFormCollection data)
        {
            return Ok(await _imageService.SaveImages(data, User));
        }


        [Authorize("User")]
        [HttpPut("images")]
        public async Task<IActionResult> UpdateImage([FromForm] IFormCollection data)
        {
            return Ok(await _imageService.UpdateImages(data, User));
        }
    
    }
}
