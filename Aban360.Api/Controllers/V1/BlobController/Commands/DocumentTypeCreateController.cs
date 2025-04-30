using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document-Type")]
    public class DocumentTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentTypeCreateHandler _documentTypeCreateHandler;
        public DocumentTypeCreateController(
            IUnitOfWork uow,
            IDocumentTypeCreateHandler documentTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentTypeCreateHandler = documentTypeCreateHandler;
            _documentTypeCreateHandler.NotNull(nameof(documentTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
