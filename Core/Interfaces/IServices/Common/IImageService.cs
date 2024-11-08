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
        Task<APIResult<string>> SaveImage(ClaimsPrincipal claims, IFormFile image);
        Task<APIResult<string>> UpdateImage(ClaimsPrincipal claims, IFormCollection data);
    }
}
