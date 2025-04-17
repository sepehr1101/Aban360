using Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.DMS.Queries
{
    [Route("v1/document-entity")]
    public class DocumentEntityGetSingleController : BaseController
    {
        private readonly IDocumentEntityGetSingleHandler _documentEntityGetSingleHandler;
        public DocumentEntityGetSingleController(IDocumentEntityGetSingleHandler documentEntityGetSingleHandler)
        {
            _documentEntityGetSingleHandler = documentEntityGetSingleHandler;
            _documentEntityGetSingleHandler.NotNull(nameof(documentEntityGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var documentEntitys = await _documentEntityGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentEntitys);
        }
    }
}
