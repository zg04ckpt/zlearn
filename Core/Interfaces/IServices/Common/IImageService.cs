using Core.Common;
using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Common
{
    public interface IImageService
    {
        Task<APIResult<IEnumerable<FileDTO>>> SaveImages(IFormCollection images, ClaimsPrincipal claimsPrincipal);
        Task<APIResult<IEnumerable<FileDTO>>> UpdateImages(IFormCollection images, ClaimsPrincipal claimsPrincipal);
    }
}
