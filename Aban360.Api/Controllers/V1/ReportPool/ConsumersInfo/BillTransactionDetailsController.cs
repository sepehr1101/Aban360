using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillTransactionDetailsController : BaseController
    {
        private readonly IBillTransactionDetailsGetHandler _billTransactionDetailsGetHandler;
        public BillTransactionDetailsController(IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler)
        {
            _billTransactionDetailsGetHandler = billTransactionDetailsGetHandler;
            _billTransactionDetailsGetHandler.NotNull(nameof(billTransactionDetailsGetHandler));
        }

        [HttpPost]
        [Route("transaction-details/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info(string billId, CancellationToken cancellationToken)
        {
            ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto> result = await _billTransactionDetailsGetHandler.Handle(billId, cancellationToken);
            return Ok(result);
        }
    }
}
