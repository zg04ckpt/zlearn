using API.Authorization;
using API.Entities;
using Core.DTOs;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ICategoryService _categoryService;

        public DocumentsController(IDocumentService documentService, ICategoryService categoryService)
        {
            _documentService = documentService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging([FromQuery] DocumentSearchingDTO data)
        {
            return Ok(await _documentService.GetAsList(data));
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDetail(string documentId)
        {
            return Ok(await _documentService.GetDetail(documentId));
        }

        [HttpGet("{documentId}/update")]
        [Authorize]
        public async Task<IActionResult> GetUpdateContent(string documentId)
        {
            return Ok(await _documentService.GetUpdateContent(documentId, User));
        }

        [HttpGet("my-documents")]
        [Authorize]
        public async Task<IActionResult> GetMyDocuments()
        {
            return Ok(await _documentService.GetMyTests(User));
        }

        [HttpGet("{documentId}/download")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(string documentId)
        {
            var res = await _documentService.DownloadDocument(documentId);
            return File(res.Stream, res.Mime, res.FileName);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _categoryService.GetChildrenCategoriesBySlug("tai-lieu"));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateDocumentDTO data)
        {
            return Ok(await _documentService.CreateNewDocument(data, User));
        }

        [HttpPut("{documentId}")]
        [Authorize]
        public async Task<IActionResult> Update(string documentId, [FromForm] UpdateDocumentDTO data)
        {
            return Ok(await _documentService.UpdateDocument(documentId, User, data));
        }

        [HttpDelete("{documentId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string documentId)
        {
            return Ok(await _documentService.DeleteDocument(documentId, User));
        }
    }
}
