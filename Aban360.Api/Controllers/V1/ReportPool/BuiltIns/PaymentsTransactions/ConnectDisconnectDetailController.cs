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
    public class ConnectDisconnectDetailController : BaseController
    {
        private readonly IConnectDisconnectDetailHandler _connectDisconnectDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConnectDisconnectDetailController(
            IConnectDisconnectDetailHandler connectDisconnectDetailHandler,
            IReportGenerator reportGenerator)
        {
            _connectDisconnectDetailHandler = connectDisconnectDetailHandler;
            _connectDisconnectDetailHandler.NotNull(nameof(_connectDisconnectDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConnectDisconnectDetailHeaderOutputDto, ConnectDisconnectDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConnectDisconnectDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ConnectDisconnectDetailHeaderOutputDto, ConnectDisconnectDetailDataOutputDto> result = await _connectDisconnectDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("detail-excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConnectDisconnectDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _connectDisconnectDetailHandler.Handle, CurrentUser, ReportLiterals.ConnectDisconnectDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("detail-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConnectDisconnectDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2133;
            ReportOutput<ConnectDisconnectDetailHeaderOutputDto, ConnectDisconnectDetailDataOutputDto> result = await _connectDisconnectDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
