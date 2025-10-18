using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterInvoiceDto, LineItemsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummary2([FromBody] SearchInput searchInput)
        {
            ReportOutput<WaterInvoiceDto, LineItemsDto> waterInvoice = await _waterInvoiceHandler.Handle(searchInput.Input);
            return Ok(waterInvoice);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SearchInput inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 90;
            ReportOutput<WaterInvoiceDto, LineItemsDto> WaterInvoiceDto = await _waterInvoiceHandler.Handle(inputDto.Input);
            JsonReportId reportId = await JsonOperation.ExportToJson(WaterInvoiceDto, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
