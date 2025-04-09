using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document_category")]
    public class DocumentCategoryGetSingleController : BaseController
    {
        private readonly IDocumentCategoryGetSingleHandler _documentCategoryGetSingleHandler;
        public DocumentCategoryGetSingleController(IDocumentCategoryGetSingleHandler documentCategoryGetSingleHandler)
        {
            _documentCategoryGetSingleHandler = documentCategoryGetSingleHandler;
            _documentCategoryGetSingleHandler.NotNull(nameof(documentCategoryGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var documentCategorys = await _documentCategoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentCategorys);
        }
    }
}
