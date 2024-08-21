
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Common
{
    public class FileService : IFileService
    {
        private readonly string _folderPath;
        public const string FOLDER_NAME = "Images";
        public const string REQUEST_PATH = "/images";

        public FileService()
        {
            _folderPath = Path.Combine(Directory.GetCurrentDirectory(), FOLDER_NAME);
        }

        public async Task DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_folderPath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            if (file == null) return null;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_folderPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(output);
            return fileName;
        }

        public string GetFileUrl(string fileName)
        {
            return REQUEST_PATH + "/" + fileName;
        }
    }
}
