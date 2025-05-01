using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DMS.Commands
{
    [Route("v1/document-entity")]
    public class DocumentEntityDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityDeleteHandler _documentEntityDeleteHandler;
        public DocumentEntityDeleteController(
            IUnitOfWork uow,
            IDocumentEntityDeleteHandler documentEntityDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityDeleteHandler = documentEntityDeleteHandler;
            _documentEntityDeleteHandler.NotNull(nameof(documentEntityDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentEntityDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
