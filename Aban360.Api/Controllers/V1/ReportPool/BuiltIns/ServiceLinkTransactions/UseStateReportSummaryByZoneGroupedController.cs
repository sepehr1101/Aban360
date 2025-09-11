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
    [Route("v1/use-state-report-summary-by-zone-grouped")]
    public class UseStateReportSummaryByZoneGroupedController : BaseController
    {
        private readonly IUseStateReportSummaryByZoneGroupedHandler _useStateReportSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public UseStateReportSummaryByZoneGroupedController(
            IUseStateReportSummaryByZoneGroupedHandler useStateReportSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _useStateReportSummaryByZoneGroupedHandler = useStateReportSummaryByZoneGroupedHandler;
            _useStateReportSummaryByZoneGroupedHandler.NotNull(nameof(_useStateReportSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateReportHeaderSummaryOutputDto, ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRaw(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateReportHeaderSummaryOutputDto, ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>> useStates = await _useStateReportSummaryByZoneGroupedHandler.Handle(inputDto, cancellationToken);
            return Ok(useStates);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _useStateReportSummaryByZoneGroupedHandler.Handle, CurrentUser, ReportLiterals.UseStateReport + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
