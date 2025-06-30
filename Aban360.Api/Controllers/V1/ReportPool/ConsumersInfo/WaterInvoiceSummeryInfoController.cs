using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/water-invoice")]
    public class WaterInvoiceSummeryInfoController : BaseController
    {
        private readonly IWaterInvoiceHandler _waterInvoiceHandler;
        public WaterInvoiceSummeryInfoController(IWaterInvoiceHandler waterInvoiceHandler)
        {
            _waterInvoiceHandler = waterInvoiceHandler;
            _waterInvoiceHandler.NotNull(nameof(waterInvoiceHandler));
        }

        [HttpPost]
        [Route("summery")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterInvoiceDto>), StatusCodes.Status200OK)]
        public IActionResult GetSummary([FromBody] SearchInput searchInput)
        {
            WaterInvoiceDto waterInvoice = _waterInvoiceHandler.Handle();
            return Ok(waterInvoice);
        }

        [HttpPost]
        [Route("summery-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterInvoiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummary2([FromBody] SearchInput searchInput)
        {
            WaterInvoiceDto waterInvoice =await _waterInvoiceHandler.Handle(searchInput.Input);
            return Ok(waterInvoice);
        }
    }
}
