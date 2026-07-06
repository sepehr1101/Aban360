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
    [Route("v1/connect-disconnect-main")]
    public class ConnectDisconnectMainController : BaseController
    {
        private readonly IConnectDisconnectMainHandler _connectDisconnectMainHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConnectDisconnectMainController(
            IConnectDisconnectMainHandler connectDisconnectMainHandler,
            IReportGenerator reportGenerator)
        {
            _connectDisconnectMainHandler = connectDisconnectMainHandler;
            _connectDisconnectMainHandler.NotNull(nameof(_connectDisconnectMainHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainDataOutputDto> result = await _connectDisconnectMainHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _connectDisconnectMainHandler.Handle, CurrentUser, ReportLiterals.ConnectDisconnectMain, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2130;
            ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainDataOutputDto> result = await _connectDisconnectMainHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
