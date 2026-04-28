using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillItemsController : BaseController
    {
        private readonly IBillItemsDetailGetByBillIdHandler _billItemsDetailGetByBillIdHandler;
        public BillItemsController(
            IBillItemsDetailGetByBillIdHandler billItemsDetailGetByBillIdHandler)
        {
            _billItemsDetailGetByBillIdHandler = billItemsDetailGetByBillIdHandler;
            _billItemsDetailGetByBillIdHandler.NotNull(nameof(billItemsDetailGetByBillIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("item-details-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<BillItemsHeaderOutputDto, BillItemsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillItemDetailsRaw([FromBody] SearchByIdInput inputDto, CancellationToken cancellationToken)
        {
            FlatReportOutput<BillItemsHeaderOutputDto, BillItemsDataOutputDto> result = await _billItemsDetailGetByBillIdHandler.Handle(inputDto.Id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("item-details-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillItemDetailsSti([FromBody] SearchByIdInput inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 3050;
            FlatReportOutput<BillItemsHeaderOutputDto, BillItemsDataOutputDto> result = await _billItemsDetailGetByBillIdHandler.Handle(inputDto.Id, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
