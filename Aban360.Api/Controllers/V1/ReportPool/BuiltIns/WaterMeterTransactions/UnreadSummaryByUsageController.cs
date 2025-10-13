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
    [Route("v1/unread-summary-by-usage")]
    public class UnreadSummaryByUsageController : BaseController
    {
        private readonly IUnreadSummaryByUsageHandler _unreadSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public UnreadSummaryByUsageController(
            IUnreadSummaryByUsageHandler unreadSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _unreadSummaryByUsageHandler = unreadSummaryByUsageHandler;
            _unreadSummaryByUsageHandler.NotNull(nameof(unreadSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnreadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto> unreadSummaryByUsage = await _unreadSummaryByUsageHandler.Handle(input, cancellationToken);
            return Ok(unreadSummaryByUsage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unreadSummaryByUsageHandler.Handle, CurrentUser, ReportLiterals.UnreadSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 381;
            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto> unread = await _unreadSummaryByUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(unread, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
