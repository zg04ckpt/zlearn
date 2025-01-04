using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.IServices.Common
{
    public interface IFileService
    {
        Task<string?> Save(IFormFile file, string folderPath);
        Task Delete(string filePath);
        Task<MemoryStream> GetFileStreamAsync(string filePath);
    }
}
