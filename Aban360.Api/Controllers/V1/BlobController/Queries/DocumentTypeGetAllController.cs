using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document_Type")]
    public class DocumentTypeGetAllController : BaseController
    {
        private readonly IDocumentTypeGetAllHandler _documentTypeGetAllHandler;
        public DocumentTypeGetAllController(IDocumentTypeGetAllHandler documentTypeGetAllHandler)
        {
            _documentTypeGetAllHandler = documentTypeGetAllHandler;
            _documentTypeGetAllHandler.NotNull(nameof(documentTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentTypes = await _documentTypeGetAllHandler.Handle(cancellationToken);
            return Ok(documentTypes);
        }
    }
}
