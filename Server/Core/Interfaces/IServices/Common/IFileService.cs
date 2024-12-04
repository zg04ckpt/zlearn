using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.IServices.Common
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
        Task DeleteFile(string fileName);
        Task<string?> Save(IFormFile file, string folderPath);
        Task Delete(string filePath);
    }
}
