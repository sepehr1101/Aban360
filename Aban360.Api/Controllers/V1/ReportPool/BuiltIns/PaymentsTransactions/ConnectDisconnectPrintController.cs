using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/service-link")]
    public class ConnectDisconnectPrintController : BaseController
    {
        private readonly IConnectDisconnectPrintHandler _connectDisconnectPrint;
        private readonly IPendingPaymentsPrintsHandler _pendingPaymentsPrintsHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConnectDisconnectPrintController(
            IConnectDisconnectPrintHandler connectDisconnectPrint,
            IPendingPaymentsPrintsHandler pendingPaymentsPrintsHandler,
            IReportGenerator reportGenerator)
        {
            _connectDisconnectPrint = connectDisconnectPrint;
            _connectDisconnectPrint.NotNull(nameof(_connectDisconnectPrint));

            _pendingPaymentsPrintsHandler=pendingPaymentsPrintsHandler;
            _pendingPaymentsPrintsHandler.NotNull(nameof(pendingPaymentsPrintsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("connect-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleConnectStiReport(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2070;
            ConnectDisconnectPrintOutputDto result = await _connectDisconnectPrint.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("disconnect-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleDisconnectStiReport(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2071;
            ConnectDisconnectPrintOutputDto result = await _connectDisconnectPrint.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("connect-list-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConnectStiReport(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2070;
            ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto> result = await _pendingPaymentsPrintsHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("disconnect-list-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDisconnectStiReport(PendingPaymentsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2071;
            ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto> result = await _pendingPaymentsPrintsHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
