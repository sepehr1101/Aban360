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
    [Route("v1/unread")]
    public class UnreadController : BaseController
    {
        private readonly IUnreadHandler _unreadHandler;
        private readonly IReportGenerator _reportGenerator;
        public UnreadController(
            IUnreadHandler unreadHandler,
            IReportGenerator reportGenerator)
        {
            _unreadHandler = unreadHandler;
            _unreadHandler.NotNull(nameof(unreadHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnreadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto> unread = await _unreadHandler.Handle(input, cancellationToken);
            return Ok(unread);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unreadHandler.Handle, CurrentUser, ReportLiterals.UnreadDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(UnreadInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 380;
            ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto> unread = await _unreadHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(unread, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}