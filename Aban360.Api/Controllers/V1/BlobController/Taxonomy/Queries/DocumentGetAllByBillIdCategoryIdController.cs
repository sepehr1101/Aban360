using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document")]
    public class DocumentGetAllByBillIdCategoryIdController : BaseController
    {
        private readonly IDocumentGetByBillIdCategoryIdHandler _documentGetByBillIdCategoryIdHandler;
        public DocumentGetAllByBillIdCategoryIdController(IDocumentGetByBillIdCategoryIdHandler documentGetByBillIdCategoryIdHandler)
        {
            _documentGetByBillIdCategoryIdHandler = documentGetByBillIdCategoryIdHandler;
            _documentGetByBillIdCategoryIdHandler.NotNull(nameof(documentGetByBillIdCategoryIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("by-bill-id-category-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillIdCategoryId(string billId,short documentCategoryId,CancellationToken cancellationToken)
        {
            var documents = await _documentGetByBillIdCategoryIdHandler.Handle(documentCategoryId,billId, cancellationToken);
            return Ok(documents);
        }
    }
}
