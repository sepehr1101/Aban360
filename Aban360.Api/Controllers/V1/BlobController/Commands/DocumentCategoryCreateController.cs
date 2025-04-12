using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/document_category")]
    public class DocumentCategoryCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentCategoryCreateHandler _documentCategoryCreateHandler;
        public DocumentCategoryCreateController(
            IUnitOfWork uow,
            IDocumentCategoryCreateHandler documentCategoryCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentCategoryCreateHandler = documentCategoryCreateHandler;
            _documentCategoryCreateHandler.NotNull(nameof(documentCategoryCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentCategoryCreateDto createDto,CancellationToken cancellationToken)
        {
            await _documentCategoryCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
