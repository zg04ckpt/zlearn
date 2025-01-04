using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.Features;
using Core.Mappers;
using Data.Entities.CommonEntities;
using Data.Entities.Enums;
using Data.Entities.TestEntities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Features
{
    public class DocumentService : IDocumentService
    {
        private const string IMAGE_REQUEST_PATH = "/api/images/document/";
        private const string FILE_REQUEST_PATH = "/api/files/document/";
        
        private readonly HashSet<string> validDocTyoes = new () { ".docx", ".doc", ".pdf", ".txt", ".pptx", ".ppt", ".csv", ".xlsx" };
        private readonly string _imageFolderPath;
        private readonly string _fileFolderPath;

        private readonly IDocumentRepository _documentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUploadedFileRepository _uploadedFileRepository;
        private readonly IFileService _fileService;

        public DocumentService(IDocumentRepository documentRepository, ICategoryRepository categoryRepository, IFileService fileService, IUploadedFileRepository uploadedFileRepository)
        {
            _imageFolderPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "Document");
            _fileFolderPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Files", "Document");
            _documentRepository = documentRepository;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _uploadedFileRepository = uploadedFileRepository;
        }

        public async Task<APIResult> CreateNewDocument(CreateDocumentDTO data, ClaimsPrincipal claims)
        {
            if(await _documentRepository.IsExist(e => e.Name.Equals(data.Name)))
            {
                throw new ErrorException("Tên tài liệu đã được sử dụng!");
            }

            if(!validDocTyoes.Contains(Path.GetExtension(data.SourceFile.FileName).ToLower()))
            {
                throw new ErrorException("Định dạng tài liệu không hợp lệ!");
            }

            var doc = DocumentMapper.MapFromCreate(data);
            doc.AuthorId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            doc.AuthorName = claims.FindFirst(ClaimTypes.GivenName)!.Value;
            doc.CategorySlug = (await _categoryRepository.GetById(Guid.Parse(data.CategoryId))).Slug;
            if(data.Image != null)
            {
                doc.ImageUrl = IMAGE_REQUEST_PATH + await _fileService.Save(data.Image, _imageFolderPath);
            }
            doc.FileName = data.SourceFile.FileName;
            doc.FilePath = FILE_REQUEST_PATH + await _fileService.Save(data.SourceFile, _fileFolderPath) 
                ?? throw new ErrorException("Tệp dữ liệu trống!");

            //create preview images
            foreach (var image in data.PreviewImages)
            {
                var imageFileName = await _fileService.Save(image, _imageFolderPath)
                    ?? throw new ErrorException("Dữ liệu ảnh  minh họa trống!");
                _uploadedFileRepository.Create(new UploadedFile
                {
                    Id = Guid.NewGuid(),
                    UploadedBy = doc.AuthorId,
                    Type = FileType.Document,
                    OwnedBy = doc.Id,
                    Path = IMAGE_REQUEST_PATH + imageFileName,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                });
            }
            await _uploadedFileRepository.SaveChanges();

            _documentRepository.Create(doc);
            await _documentRepository.SaveChanges();

            return new APISuccessResult("Đã tải lên tài liệu");
        }

        public async Task<APIResult> DeleteDocument(string docummentId, ClaimsPrincipal claims)
        {
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var doc = await _documentRepository.GetById(Guid.Parse(docummentId))
                ?? throw new ErrorException("Không tìm thấy tài liệu");

            if (!doc.AuthorId.Equals(userId))
            {
                if(!claims.IsInRole("Admin"))
                {
                    throw new ForbiddenException();
                }
            }

            //remove images and file of document
            if(!string.IsNullOrEmpty(doc.ImageUrl))
            {
                await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(doc.ImageUrl)));
            }
            await _fileService.Delete(Path.Combine(_fileFolderPath, Path.GetFileName(doc.FilePath)));
            var previewImages = await _uploadedFileRepository.GetAll(e => e.OwnedBy.Equals(doc.Id));
            foreach(var img in previewImages)
            {
                await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(img.Path)));
            }
            _uploadedFileRepository.DeleteRange(previewImages.ToList());
            _documentRepository.Delete(doc);

            await _uploadedFileRepository.SaveChanges();
            await _documentRepository.SaveChanges();
            return new APISuccessResult("Xóa tài liệu thành công");
        }

        public async Task<FileStreamDTO> DownloadDocument(string docummentId)
        {
            var doc = await _documentRepository.GetById(Guid.Parse(docummentId))
                ?? throw new ErrorException("Tài liệu không tồn tại");

            //save download count
            doc.PurchaseCount = doc.PurchaseCount + 1;
            _documentRepository.Update(doc);
            await _documentRepository.SaveChanges();

            MemoryStream stream = await _fileService.GetFileStreamAsync(Path.Combine(_fileFolderPath, Path.GetFileName(doc.FilePath)));
            return new FileStreamDTO
            {
                Stream = stream,
                Mime = GetContentType(doc.FileName),
                FileName = doc.FileName
            };
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        public async Task<APIResult<PaginatedResult<DocumentItemDTO>>> GetAsList(DocumentSearchingDTO data)
        {
            var query = _documentRepository.GetQuery().AsNoTracking();

            //filter
            if (!string.IsNullOrEmpty(data.CategorySlug))
            {
                query = query.Where(e => e.CategorySlug.Equals(data.CategorySlug));
            }
            if (!string.IsNullOrEmpty(data.Name))
            {
                query = query.Where(e => e.Name.Contains(data.Name));
            }

            //paging
            var total = await query.CountAsync();
            var res = await query
                .Skip((data.PageIndex - 1) * data.PageSize)
                .Take(data.PageSize)
                .OrderByDescending(e => e.PurchaseCount)
                .ToListAsync();

            return new APISuccessResult<PaginatedResult<DocumentItemDTO>>(new PaginatedResult<DocumentItemDTO>
            {
                Data = res.Select(e => DocumentMapper.MapToItem(e)),
                Total = total
            });
        }

        public async Task<APIResult<DocumentDetailDTO>> GetDetail(string docummentId)
        {
            var doc = await _documentRepository.GetById(Guid.Parse(docummentId))
                ?? throw new ErrorException("Tài liệu không tồn tại");

            var images = await _uploadedFileRepository.GetAll(e => e.Type == FileType.Document && e.OwnedBy.Equals(doc.Id));
            return new APISuccessResult<DocumentDetailDTO>(new DocumentDetailDTO
            {
                Size = doc.Size,
                Description = doc.Description,
                PreviewImagePaths = images.Select(e => e.Path).ToList(),
            });
        }

        public async Task<APIResult<List<DocumentItemDTO>>> GetMyTests(ClaimsPrincipal claims)
        {
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tests = await _documentRepository.GetAll(e => e.AuthorId.Equals(userId));
            return new APISuccessResult<List<DocumentItemDTO>>(
                tests.Select(e => DocumentMapper.MapToItem(e)).ToList()
            );
        }

        public async Task<APIResult<UpdateDocumentDTO>> GetUpdateContent(string docummentId, ClaimsPrincipal claims)
        {
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var doc = await _documentRepository.GetById(Guid.Parse(docummentId))
                ?? throw new ErrorException("Không tìm thấy tài liệu");

            if(!doc.AuthorId.Equals(userId))
            {
                throw new ForbiddenException();
            }

            var data = DocumentMapper.MapToUpdate(doc);
            // Get preview images
            var images = await _uploadedFileRepository.GetAll(e => e.Type == FileType.Document && e.OwnedBy.Equals(doc.Id));
            data.PreviewImages = images.Select(e => new UpdateImageDTO
            {
                Id = e.Id.ToString(),
                ImageUrl = e.Path
            }).ToList();

            return new APISuccessResult<UpdateDocumentDTO>(data);
        }

        public async Task<APIResult> UpdateDocument(string docummentId, ClaimsPrincipal claims, UpdateDocumentDTO data)
        {
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var doc = await _documentRepository.GetById(Guid.Parse(docummentId))
                ?? throw new ErrorException("Không tìm thấy tài liệu");

            if (!doc.AuthorId.Equals(userId))
            {
                throw new ForbiddenException();
            }

            //Update
            doc.Name = data.Name;
            doc.Description = data.Description;
            doc.CategoryId = Guid.Parse(data.CategoryId);
            doc.CategorySlug = (await _categoryRepository.GetById(Guid.Parse(data.CategoryId))).Slug;
            doc.UpdatedAt = DateTime.Now;

            //update image
            if (data.NewImage != null)
            {
                if(!string.IsNullOrEmpty(doc.ImageUrl))
                {
                    await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(doc.ImageUrl)));
                }
                doc.ImageUrl = IMAGE_REQUEST_PATH + await _fileService.Save(data.NewImage, _imageFolderPath);
            }

            //update file
            if (data.NewSourceFile != null)
            {
                doc.FileName = data.NewSourceFile.FileName;
                doc.Size = (int)data.NewSourceFile.Length;
                await _fileService.Delete(Path.Combine(_fileFolderPath, Path.GetFileName(doc.FilePath)));
                doc.FilePath = FILE_REQUEST_PATH + await _fileService.Save(data.NewSourceFile, _fileFolderPath)
                    ?? throw new ErrorException("Tệp dữ liệu trống!");
            }

            //update preview images
            var images = await _uploadedFileRepository.GetAll(e => e.Type == FileType.Document && e.OwnedBy.Equals(doc.Id));
            HashSet<string> set = new ();
            foreach (var image in data.PreviewImages)
            {
                if(image.Id == null)
                {
                    var imageFileName = await _fileService.Save(image.NewImage!, _imageFolderPath)
                    ?? throw new ErrorException("Dữ liệu ảnh  minh họa trống!");
                    _uploadedFileRepository.Create(new UploadedFile
                    {
                        Id = Guid.NewGuid(),
                        UploadedBy = doc.AuthorId,
                        Type = FileType.Document,
                        OwnedBy = doc.Id,
                        Path = IMAGE_REQUEST_PATH + imageFileName,
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    });
                }
                else
                {
                    set.Add(image.Id!);
                }    
            }
            foreach(var image in images)
            {
                if(!set.Contains(image.Id.ToString()))
                {
                    await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(image.Path)));
                    _uploadedFileRepository.Delete(image);
                }    
            }

            //save changes
            await _uploadedFileRepository.SaveChanges();
            await _documentRepository.SaveChanges();
            return new APISuccessResult("Cập nhật thành công");
        }
    }
}
