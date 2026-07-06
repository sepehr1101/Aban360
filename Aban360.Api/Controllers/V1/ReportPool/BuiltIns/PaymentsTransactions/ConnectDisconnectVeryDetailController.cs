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
    [Route("v1/connect-disconnect-report")]
    public class ConnectDisconnectVeryDetailController : BaseController
    {
        private readonly IConnectDisconnectVeryDetailHandler _connectDisconnectVeryDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConnectDisconnectVeryDetailController(
            IConnectDisconnectVeryDetailHandler connectDisconnectVeryDetailHandler,
            IReportGenerator reportGenerator)
        {
            _connectDisconnectVeryDetailHandler = connectDisconnectVeryDetailHandler;
            _connectDisconnectVeryDetailHandler.NotNull(nameof(_connectDisconnectVeryDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("vary-detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConnectDisconnectVeryDetailHeaderOutputDto, ConnectDisconnectVeryDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConnectDisconnectVeryDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ConnectDisconnectVeryDetailHeaderOutputDto, ConnectDisconnectVeryDetailDataOutputDto> result = await _connectDisconnectVeryDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("vary-detail-excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConnectDisconnectVeryDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _connectDisconnectVeryDetailHandler.Handle, CurrentUser, ReportLiterals.ConnectDisconnectVeryDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("vary-detail-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConnectDisconnectVeryDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2134;
            ReportOutput<ConnectDisconnectVeryDetailHeaderOutputDto, ConnectDisconnectVeryDetailDataOutputDto> result = await _connectDisconnectVeryDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
