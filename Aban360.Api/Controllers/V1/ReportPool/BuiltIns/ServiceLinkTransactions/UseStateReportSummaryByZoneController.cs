using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/use-state-report-summary-by-zone")]
    public class UseStateReportSummaryByZoneController : BaseController
    {
        private readonly IUseStateReportSummaryByZoneHandler _useStateReportSummaryByZoneHandler;
        private readonly IReportGenerator _reportGenerator;
        public UseStateReportSummaryByZoneController(
            IUseStateReportSummaryByZoneHandler useStateReportSummaryByZoneHandler,
            IReportGenerator reportGenerator)
        {
            _useStateReportSummaryByZoneHandler = useStateReportSummaryByZoneHandler;
            _useStateReportSummaryByZoneHandler.NotNull(nameof(_useStateReportSummaryByZoneHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRaw(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto> useStates = await _useStateReportSummaryByZoneHandler.Handle(inputDto, cancellationToken);
            return Ok(useStates);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _useStateReportSummaryByZoneHandler.Handle, CurrentUser, ReportLiterals.UseStateReport+ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 62;
            ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto> useStates = await _useStateReportSummaryByZoneHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(useStates, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
