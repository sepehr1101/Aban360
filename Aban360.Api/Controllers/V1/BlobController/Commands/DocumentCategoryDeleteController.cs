using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document-category")]
    public class DocumentCategoryDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentCategoryDeleteHandler _documentCategoryDeleteHandler;
        public DocumentCategoryDeleteController(
            IUnitOfWork uow,
            IDocumentCategoryDeleteHandler documentCategoryDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentCategoryDeleteHandler = documentCategoryDeleteHandler;
            _documentCategoryDeleteHandler.NotNull(nameof(documentCategoryDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentCategoryDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
