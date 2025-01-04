using Core.Common;
using Core.DTOs;
using Data.Entities.DocumentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
    public class DocumentMapper
    {
        public static Document MapFromCreate(CreateDocumentDTO data)
        {
            return new Document
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Description = data.Description,
                PurchaseCount = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                PaymentInfoId = null,
                Slug = Utilities.CreateSlugFromString(data.Name),
                CategoryId = Guid.Parse(data.CategoryId),
                Size = (int)data.SourceFile.Length
            };
        }

        public static DocumentItemDTO MapToItem(Document document)
        {
            return new DocumentItemDTO
            {
                Id = document.Id.ToString(),
                Name = document.Name,
                ImageUrl = document.ImageUrl,
                AuthorName = document.AuthorName,
                AuthorId = document.AuthorId.ToString(),
                UpdatedAt = document.UpdatedAt,
                DownloadedCount = document.PurchaseCount,
                OriginPrice = 0,
                LastPrice = 0,
                DocumentType = Path.GetExtension(document.FileName).ToUpper()
            };
        }

        public static UpdateDocumentDTO MapToUpdate(Document document)
        {
            return new UpdateDocumentDTO
            {
                Name = document.Name,
                FileName = document.FileName,
                CategoryId = document.CategoryId.ToString(),
                Description = document.Description,
                ImageUrl = document.ImageUrl,
            };
        }
    }
}
