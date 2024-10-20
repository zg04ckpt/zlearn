using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Common
{
    public class ImageService : IImageService
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;
        public ImageService(IFileService fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        public async Task<APIResult<IEnumerable<FileDTO>>> SaveImages(IFormCollection images, ClaimsPrincipal claimsPrincipal)
        {
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var data = new Dictionary<string, IFormFile>();
            foreach (var file in images.Files)
            {
                data[file.Name] = file;
            }

            var savedImageNames = new List<string>();
            var result = new List<FileDTO>();
            foreach (var e in data)
            {
                string imageName = await _fileService.SaveFile(e.Value);
                savedImageNames.Add(imageName);
                result.Add(new FileDTO
                {
                    Key = e.Key,
                    Url = _fileService.GetFileUrl(imageName)
                });
                
            }
            _context.Images.AddRange(savedImageNames.Select(e => new Image
            {
                Name = e,
                Owner = userId
            }));
            await _context.SaveChangesAsync();
            return new APISuccessResult<IEnumerable<FileDTO>>("Lưu danh sách ảnh thành công", result);
        }

        public async Task<APIResult<IEnumerable<FileDTO>>> UpdateImages(IFormCollection images, ClaimsPrincipal claimsPrincipal)
        {
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var data = new Dictionary<string, IFormFile>();
            foreach (var file in images.Files)
            {
                data[file.Name] = file;
            }

            var savedImageNames = new List<string>();
            var result = new List<FileDTO>();
            foreach (var e in data)
            {
                //xóa ảnh cũ
                string oldFileName = Path.GetFileName(e.Key);
                await _fileService.DeleteFile(oldFileName);
                var oldImage = await _context.Images.FirstAsync(x => x.Name.Equals(oldFileName) && x.Owner.Equals(userId))
                    ?? throw new ErrorException("Không tìm thấy ảnh hoặc bạn không có quyền sửa đổi");
                _context.Images.Remove(oldImage);
                //Tạo ảnh mới
                string imageName = await _fileService.SaveFile(e.Value);
                savedImageNames.Add(imageName);
                result.Add(new FileDTO
                {
                    Key = e.Key,
                    Url = _fileService.GetFileUrl(imageName)
                });

            }
            _context.Images.AddRange(savedImageNames.Select(e => new Image
            {
                Name = e,
                Owner = userId
            }));
            await _context.SaveChangesAsync();
            return new APISuccessResult<IEnumerable<FileDTO>>("Cập nhật danh sách ảnh thành công", result);
        }
    }
}
