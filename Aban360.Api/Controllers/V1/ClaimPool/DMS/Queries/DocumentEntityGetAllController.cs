using Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.DMS.Queries
{
    [Route("v1/document-entity")]
    public class DocumentEntityGetAllController : BaseController
    {
        private readonly IDocumentEntityGetAllHandler _documentEntityGetAllHandler;
        public DocumentEntityGetAllController(IDocumentEntityGetAllHandler documentEntityGetAllHandler)
        {
            _documentEntityGetAllHandler = documentEntityGetAllHandler;
            _documentEntityGetAllHandler.NotNull(nameof(documentEntityGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentEntityGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentEntitys = await _documentEntityGetAllHandler.Handle(cancellationToken);
            return Ok(documentEntitys);
        }
    }
}
