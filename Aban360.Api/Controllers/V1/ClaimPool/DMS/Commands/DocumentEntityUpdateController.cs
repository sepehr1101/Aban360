using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.DMS.Commands
{
    [Route("v1/document-entity")]
    public class DocumentEntityUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityUpdateHandler _documentEntityUpdateHandler;
        public DocumentEntityUpdateController(
            IUnitOfWork uow,
            IDocumentEntityUpdateHandler documentEntityUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityUpdateHandler = documentEntityUpdateHandler;
            _documentEntityUpdateHandler.NotNull(nameof(documentEntityUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentEntityUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
