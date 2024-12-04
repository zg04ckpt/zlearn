using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Common
{
    public class FileService : IFileService
    {
        private readonly string _folderPath;
        public const string IMAGE_FOLDER_NAME = "Images";
        public const string IMAGE_PATH = "/api/images/test";

        public FileService()
        {
            _folderPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "Test");
        }

        public async Task DeleteFile(string fileName)
        {
            if (fileName == null || fileName.Equals("default.jpg"))
            {
                return;
            }
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
                return null;
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_folderPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(output);
            return fileName;
        }

        public async Task<string?> Save(IFormFile file, string folderPath)
        {
            if (file == null)
            {
                return null;
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(output);
            return fileName;
        }

        public async Task Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
