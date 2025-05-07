using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document-category")]
    public class DocumentCategoryGetAllByBillIdController : BaseController
    {
        private readonly IDocumentCategoryGetByBillIdHandler _documentCategoryGetBillIdHandler;
        public DocumentCategoryGetAllByBillIdController(IDocumentCategoryGetByBillIdHandler documentCategoryGetBillIdHandler)
        {
            _documentCategoryGetBillIdHandler = documentCategoryGetBillIdHandler;
            _documentCategoryGetBillIdHandler.NotNull(nameof(documentCategoryGetBillIdHandler));
        }

        [HttpPost]
        [Route("bill-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ByBillId([FromBody] SearchInput inputDate,CancellationToken cancellationToken)
        {
            var documentCategories = await _documentCategoryGetBillIdHandler.Handle(inputDate.Input, cancellationToken);
            return Ok(documentCategories);
        }
    }
}
