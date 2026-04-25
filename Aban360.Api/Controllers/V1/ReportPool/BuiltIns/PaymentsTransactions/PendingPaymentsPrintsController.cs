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
    [Route("v1/pending-payments-prints")]
    public class PendingPaymentsPrintsController : BaseController
    {
        private readonly IPendingPaymentsPrintsHandler _pendingPaymentsPrintsHandler;
        private readonly IReportGenerator _reportGenerator;
        public PendingPaymentsPrintsController(
            IPendingPaymentsPrintsHandler pendingPaymentsPrintsHandler,
            IReportGenerator reportGenerator)
        {
            _pendingPaymentsPrintsHandler = pendingPaymentsPrintsHandler;
            _pendingPaymentsPrintsHandler.NotNull(nameof(_pendingPaymentsPrintsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _pendingPaymentsPrintsHandler.Handle, CurrentUser, ReportLiterals.PendingPayments, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2030;
            ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto> result = await _pendingPaymentsPrintsHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
