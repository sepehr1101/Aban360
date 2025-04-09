using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document_Type")]
    public class DocumentTypeGetSingleController : BaseController
    {
        private readonly IDocumentTypeGetSingleHandler _documentTypeGetSingleHandler;
        public DocumentTypeGetSingleController(IDocumentTypeGetSingleHandler documentTypeGetSingleHandler)
        {
            _documentTypeGetSingleHandler = documentTypeGetSingleHandler;
            _documentTypeGetSingleHandler.NotNull(nameof(documentTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var documentTypes = await _documentTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentTypes);
        }
    }
}
