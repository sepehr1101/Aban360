using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Microsoft.AspNetCore.Mvc;
using IMeterFlowLookupService = global::Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterFlowLookupService;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillTransactionDetailsController : BaseController
    {
        private readonly IBillTransactionDetailsGetHandler _billTransactionDetailsGetHandler;
        private readonly IMeterFlowLookupService _meterFlowLookupService;
        public BillTransactionDetailsController(IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler, IMeterFlowLookupService meterFlowLookupService)
        {
            _billTransactionDetailsGetHandler = billTransactionDetailsGetHandler;
            _billTransactionDetailsGetHandler.NotNull(nameof(billTransactionDetailsGetHandler));
            _meterFlowLookupService = meterFlowLookupService;
            _meterFlowLookupService.NotNull(nameof(meterFlowLookupService));
        }

        [HttpPost, HttpGet]
        [Route("transaction-details/{billid}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info(string billid, CancellationToken cancellationToken)
        {
            ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto> result = await _billTransactionDetailsGetHandler.Handle(billid, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("transaction-details-reading-number/{readingNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByReadingNumber(string readingNumber, CancellationToken cancellationToken)
        {
            ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto> result = await _meterFlowLookupService.MeterReadingDetailLatestInfoByReadingNumberGetAsync(readingNumber, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
