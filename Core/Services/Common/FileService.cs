using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Common
{
    public class FileService : IFileService
    {
        private readonly string _folderPath;
        public const string IMAGE_FOLDER_NAME = "Images";
        public const string IMAGE_PATH = "/api/images";

        public FileService()
        {
            _folderPath = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_FOLDER_NAME);
        }

        public async Task DeleteFile(string fileName)
        {
            if (fileName == null)
                throw new ErrorException("Nội dung file trống");
            var filePath = Path.Combine(_folderPath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            if (file == null)
            {
                throw new ErrorException("File trống");
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_folderPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(output);
            return fileName;
        }

        public string GetFileUrl(string fileName)
        {
            if (fileName == null) return null;
            return IMAGE_PATH + "/" + fileName;
        }

        public static string GetAsUrl(string fileName)
        {
            if (fileName == null) return null;
            return IMAGE_PATH + "/" + fileName;
        }
    }
}
