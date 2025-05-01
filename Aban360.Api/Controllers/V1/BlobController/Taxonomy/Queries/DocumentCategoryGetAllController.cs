using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document-category")]
    public class DocumentCategoryGetAllController : BaseController
    {
        private readonly IDocumentCategoryGetAllHandler _documentCategoryGetAllHandler;
        public DocumentCategoryGetAllController(IDocumentCategoryGetAllHandler documentCategoryGetAllHandler)
        {
            _documentCategoryGetAllHandler = documentCategoryGetAllHandler;
            _documentCategoryGetAllHandler.NotNull(nameof(documentCategoryGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentCategorys = await _documentCategoryGetAllHandler.Handle(cancellationToken);
            return Ok(documentCategorys);
        }
    }
}
