using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public ImageService(IFileService fileService, AppDbContext context, UserManager<AppUser> userManager)
        {
            _fileService = fileService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<APIResult<string>> SaveImage(ClaimsPrincipal claims, IFormFile image)
        {
            string userId = claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            string imageFileName = await _fileService.SaveFile(image);
            _context.Images.Add(new Image
            {
                Name = imageFileName,
                Owner = userId
            });
            await _context.SaveChangesAsync();

            return new APISuccessResult<string>(FileService.GetAsUrl(imageFileName));
        }

        public async Task<APIResult<string>> UpdateImage(ClaimsPrincipal claims, IFormCollection data)
        {
            string userId = claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string imageFileName = Path.GetFileName(data["url"]);
            IFormFile image = data.Files["image"];

            var old = await _context.Images.FindAsync(imageFileName)
                ?? throw new ErrorException("Không tìm thấy ảnh cần cập nhật");
            if (!old.Owner.Equals(userId))
            {
                throw new ErrorException("Không có quyền cập nhật ảnh này");
            }
            _context.Images.Remove(old);
            await _fileService.DeleteFile(imageFileName);
            imageFileName = await _fileService.SaveFile(image);
            _context.Images.Add(new Image
            {
                Name = imageFileName,
                Owner = userId
            });
            await _context.SaveChangesAsync();

            return new APISuccessResult<string>(FileService.GetAsUrl(imageFileName));
        }
    }
}
