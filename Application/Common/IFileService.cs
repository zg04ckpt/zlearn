using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Features.Learn.Test;

namespace Application.Common
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
        Task DeleteFile(string fileName);
        string GetFileUrl(string fileName);
    }
}
