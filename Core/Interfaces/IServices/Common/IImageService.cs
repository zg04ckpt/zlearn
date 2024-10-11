using Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Common
{
    public interface IImageService
    {
        Task<APIResult<string>> Create(IFormFile image);
        Task<APIResult<string>> Update(IFormFile newImage, string oldImageUrl);
    }
}
