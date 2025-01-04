using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateDocumentDTO
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile SourceFile { get; set; }
        public List<IFormFile> PreviewImages { get; set; }
        public PaymentInfoDTO? PaymentInfo { get; set; }
    }

    public class UpdateDocumentDTO
    {
        public string Name { get; set; }
        public string? FileName { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? NewImage { get; set; }
        public IFormFile? NewSourceFile { get; set; }
        public List<UpdateImageDTO> PreviewImages { get; set; }
        public PaymentInfoDTO? PaymentInfo { get; set; }
    }

    public class UpdateImageDTO
    {
        public string? Id { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? NewImage { get; set; }
    }

    public class PaymentInfoDTO
    {

    }

    public class DocumentDetailDTO
    {
        public int Size { get; set; } //Byte
        public string Description { get; set; }
        public List<string> PreviewImagePaths { get; set; }
    }

    public class DocumentItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string DocumentType { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int DownloadedCount { get; set; }
        public int OriginPrice { get; set; }
        public int LastPrice { get; set; }
    }

    public class DocumentSearchingDTO
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? CategorySlug { get; set; }
    }
}
