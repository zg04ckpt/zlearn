using Core.Common;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IServices.Features
{
    public interface IDocumentService
    {
        Task<APIResult> CreateNewDocument(CreateDocumentDTO data, ClaimsPrincipal claims);
        Task<APIResult<PaginatedResult<DocumentItemDTO>>> GetAsList(DocumentSearchingDTO data);
        Task<APIResult<DocumentDetailDTO>> GetDetail(string docummentId);
        Task<APIResult<List<DocumentItemDTO>>> GetMyTests(ClaimsPrincipal claims);
        Task<APIResult<UpdateDocumentDTO>> GetUpdateContent(string docummentId, ClaimsPrincipal claims);
        Task<APIResult> UpdateDocument(string docummentId, ClaimsPrincipal claims, UpdateDocumentDTO data);
        Task<APIResult> DeleteDocument(string docummentId, ClaimsPrincipal claims);
        Task<FileStreamDTO> DownloadDocument(string docummentId);
    }
}
