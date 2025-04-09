using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document_Type")]
    public class DocumentTypeDeleteController : BaseController
    {
        private readonly IUnitOfwork _uow;
        private readonly IDocumentTypeDeleteHandler _documentTypeDeleteHandler;
        public DocumentTypeDeleteController(
            IUnitOfwork uow,
            IDocumentTypeDeleteHandler documentTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentTypeDeleteHandler = documentTypeDeleteHandler;
            _documentTypeDeleteHandler.NotNull(nameof(documentTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
