using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DMS.Commands
{
    [Route("v1/document-entity")]
    public class DocumentEntityCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityCreateHandler _documentEntityCreateHandler;
        private readonly IDocumentCreateHandler _documentCreateHandler;
        public DocumentEntityCreateController(
            IUnitOfWork uow,
            IDocumentEntityCreateHandler documentEntityCreateHandler,
            IDocumentCreateHandler documentCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityCreateHandler = documentEntityCreateHandler;
            _documentEntityCreateHandler.NotNull(nameof(documentEntityCreateHandler));

            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));

        }
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityByDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentEntityByDocumentCreateDto documentEntityByDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentCreateHandler.Handle(documentEntityByDocumentCreateDto.documentCreateDto.DocumentFile, documentEntityByDocumentCreateDto.documentCreateDto.DocumentTypeId, documentEntityByDocumentCreateDto.documentCreateDto.Description, cancellationToken);
            await _documentEntityCreateHandler.Handle(documentEntityByDocumentCreateDto.documentEntityCreateDto, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(documentEntityByDocumentCreateDto);
        }
    }
}
