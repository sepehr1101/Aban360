using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document_Type")]
    public class DocumentTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentTypeUpdateHandler _documentTypeUpdateHandler;
        public DocumentTypeUpdateController(
            IUnitOfWork uow,
            IDocumentTypeUpdateHandler documentTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentTypeUpdateHandler = documentTypeUpdateHandler;
            _documentTypeUpdateHandler.NotNull(nameof(documentTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] DocumentTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
