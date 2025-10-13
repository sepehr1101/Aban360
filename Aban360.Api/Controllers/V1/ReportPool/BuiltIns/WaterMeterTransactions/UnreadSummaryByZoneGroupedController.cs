using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/unread-summary-by-zone-grouped")]
    public class UnreadSummaryByZoneGroupedController : BaseController
    {
        private readonly IUnreadSummaryByZoneGroupedHandler _unreadSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public UnreadSummaryByZoneGroupedController(
            IUnreadSummaryByZoneGroupedHandler unreadSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _unreadSummaryByZoneGroupedHandler = unreadSummaryByZoneGroupedHandler;
            _unreadSummaryByZoneGroupedHandler.NotNull(nameof(unreadSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnreadSummaryHeaderOutputDto, ReportOutput<UnreadSummaryDataOutputDto, UnreadSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnreadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnreadSummaryHeaderOutputDto, ReportOutput<UnreadSummaryDataOutputDto, UnreadSummaryDataOutputDto>> unreadSummaryByZoneGrouped = await _unreadSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(unreadSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unreadSummaryByZoneGroupedHandler.HandleFlat, CurrentUser, ReportLiterals.UnreadSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 383;
            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto> unread = await _unreadSummaryByZoneGroupedHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(unread, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
