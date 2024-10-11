using Core.Common;
using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Services.Common
{
    public class ImageService : IImageService
    {
        private readonly IFileService _fileService;
        public ImageService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<APIResult<string>> Create(IFormFile image)
        {
            string imageName = await _fileService.SaveFile(image);
            return new APISuccessResult<string>("Lưu ảnh thành công", imageName);
        }

        public async Task<APIResult<string>> Update(IFormFile newImage, string oldImageUrl)
        {
            if(!await _fileService.DeleteFile(Path.GetFileName(oldImageUrl)))
            {
                throw new ErrorException("Xóa ảnh cũ thất bại");
            }
            string imageName = await _fileService.SaveFile(newImage);
            return new APISuccessResult<string>("Cập nhật ảnh thành công", imageName);
        }
    }
}
