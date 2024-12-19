using Core.Exceptions;
using Core.Interfaces.IServices.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Common
{
    public class FileService : IFileService
    {
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

        public async Task<MemoryStream> GetFileStreamAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ErrorException("Không tìm thấy tài liệu");
            }
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;
        }
    }
}
