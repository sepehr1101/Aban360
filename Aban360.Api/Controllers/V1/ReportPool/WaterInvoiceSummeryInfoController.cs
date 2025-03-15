using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("v1/water-invoice")]
    public class WaterInvoiceSummeryInfoController : BaseController
    {
        private readonly IWaterInvoiceQueryService _waterInvoiceQueryService;
        public WaterInvoiceSummeryInfoController(IWaterInvoiceQueryService waterInvoiceQueryService)
        {
            _waterInvoiceQueryService = waterInvoiceQueryService;
            _waterInvoiceQueryService.NotNull(nameof(waterInvoiceQueryService));
        }

        [HttpPost]
        [Route("summery")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterInvoiceDto>), StatusCodes.Status200OK)]
        public IActionResult GetSummary([FromBody] SearchInput searchInput)
        {
            WaterInvoiceDto waterInvoice = _waterInvoiceQueryService.Get();
            return Ok(waterInvoice);
        }
    }
}
