using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/pending-payments-summary-by-zone-usage")]
    public class PendingPaymentsSummaryByZoneAndUsageController : BaseController
    {
        private readonly IPendingPaymentsSummaryByZoneAndUsageHandler _pendingPaymentsHandler;
        private readonly IReportGenerator _reportGenerator;
        public PendingPaymentsSummaryByZoneAndUsageController(
            IPendingPaymentsSummaryByZoneAndUsageHandler pendingPaymentsHandler,
            IReportGenerator reportGenerator)
        {
            _pendingPaymentsHandler = pendingPaymentsHandler;
            _pendingPaymentsHandler.NotNull(nameof(_pendingPaymentsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryByZoneAndUsageDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryByZoneAndUsageDataOutputDto> pendingPayments = await _pendingPaymentsHandler.Handle(inputDto, cancellationToken);
            return Ok(pendingPayments);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _pendingPaymentsHandler.Handle, CurrentUser, ReportLiterals.PendingPaymentsSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 502;
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryByZoneAndUsageDataOutputDto> result = await _pendingPaymentsHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
