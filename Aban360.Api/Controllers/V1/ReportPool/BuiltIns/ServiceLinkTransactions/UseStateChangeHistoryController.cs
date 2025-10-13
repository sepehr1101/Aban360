using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/use-state-change-history")]
    public class UseStateChangeHistoryController : BaseController
    {
        private readonly IDeletionStateChangeHistoryHandler _deletionStateChangeHistory;
        private readonly IReportGenerator _reportGenerator;
        public UseStateChangeHistoryController(
            IDeletionStateChangeHistoryHandler deletionStateChangeHistory,
            IReportGenerator reportGenerator)
        {
            _deletionStateChangeHistory = deletionStateChangeHistory;
            _deletionStateChangeHistory.NotNull(nameof(_deletionStateChangeHistory));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DeletionStateChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> DeletionStateChangeHistory = await _deletionStateChangeHistory.Handle(inputDto, cancellationToken);
            return Ok(DeletionStateChangeHistory);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, DeletionStateChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _deletionStateChangeHistory.Handle, CurrentUser, ReportLiterals.DeletionStateChangeHistory, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(DeletionStateChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 67;
            ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> DeletionStateChangeHistory = await _deletionStateChangeHistory.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(DeletionStateChangeHistory, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
