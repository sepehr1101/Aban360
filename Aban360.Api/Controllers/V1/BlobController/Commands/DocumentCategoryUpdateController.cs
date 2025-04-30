using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document-category")]
    public class DocumentCategoryUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentCategoryUpdateHandler _documentCategoryUpdateHandler;
        public DocumentCategoryUpdateController(
            IUnitOfWork uow,
            IDocumentCategoryUpdateHandler documentCategoryUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentCategoryUpdateHandler = documentCategoryUpdateHandler;
            _documentCategoryUpdateHandler.NotNull(nameof(documentCategoryUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentCategoryUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
