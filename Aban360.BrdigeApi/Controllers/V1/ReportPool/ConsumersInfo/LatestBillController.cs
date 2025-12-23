using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]//تکمیل مستندات
    public class LatestBillController : BaseController
    {
        private readonly IWaterInvoiceHandler _waterInvoiceHandler;
        public LatestBillController(
            IWaterInvoiceHandler waterInvoiceHandler)
        {
            _waterInvoiceHandler = waterInvoiceHandler;
            _waterInvoiceHandler.NotNull(nameof(waterInvoiceHandler));
        }


        [HttpPost]
        [Route("latest")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterInvoiceDto, LineItemsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterInvoiceDto, LineItemsDto> result = await _waterInvoiceHandler.Handle_WithLastDb(input.Input, cancellationToken);
            return Ok(result);
        }
    }
}
