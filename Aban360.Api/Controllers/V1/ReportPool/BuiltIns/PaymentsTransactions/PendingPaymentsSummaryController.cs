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
    [Route("v1/pending-payments-summary")]
    public class PendingPaymentsSummaryController : BaseController
    {
        private readonly IPendingPaymentsSummaryHandler _pendingPaymentsHandler;
        private readonly IReportGenerator _reportGenerator;
        public PendingPaymentsSummaryController(
            IPendingPaymentsSummaryHandler pendingPaymentsHandler,
            IReportGenerator reportGenerator)
        {
            _pendingPaymentsHandler = pendingPaymentsHandler;
            _pendingPaymentsHandler.NotNull(nameof(_pendingPaymentsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PendingPaymentsSummaryDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto> pendingPayments = await _pendingPaymentsHandler.Handle(inputDto, cancellationToken);
            return Ok(pendingPayments);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PendingPaymentsSummaryDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _pendingPaymentsHandler.Handle, CurrentUser, ReportLiterals.PendingPaymentsSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(PendingPaymentsSummaryDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 501;
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto> result = await _pendingPaymentsHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
