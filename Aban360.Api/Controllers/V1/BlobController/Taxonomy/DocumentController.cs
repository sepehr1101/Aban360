using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Application.Features.Taxonomy.Services;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy
{
    [Route("v1/document")]
    public class DocumentController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentService _documentService;
        public DocumentController(
        IUnitOfWork uow,
        IDocumentService documentService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
            _documentService.NotNull(nameof(_documentService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentService.CreateAsync(createDto.DocumentFile, createDto.DocumentTypeId, createDto.Description, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [Route("single/{id}")]
        [HttpPost, HttpGet]
        public async Task<FileResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var y = await _documentService.GetSingleAsync(id, cancellationToken);
            return File(y.FileContent, y.ContentType);
        }

        [HttpPost, HttpGet]
        [Route("by-bill-id-category-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillIdCategoryId(string billId, short documentCategoryId, CancellationToken cancellationToken)
        {
            var documents = await _documentService.GetByBillAndCategoryAsync(documentCategoryId, billId, cancellationToken);
            return Ok(documents);
        }
    }
}
