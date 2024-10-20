using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.IServices.Common
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
        Task DeleteFile(string fileName);
        string GetFileUrl(string fileName);
    }
}
