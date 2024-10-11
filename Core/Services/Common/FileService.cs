using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Consts;

namespace Core.Services.Common
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

        public async Task<bool> DeleteFile(string fileName)
        {
            if (fileName == null) return false;
            var filePath = Path.Combine(_folderPath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            return false;
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
            return REQUEST_PATH + "/" + fileName;
        }

        public static string GetAsUrl(string fileName)
        {
            if (fileName == null) return null;
            return REQUEST_PATH + "/" + fileName;
        }
    }
}
