using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/branch-type-change-history")]
    public class BranchTypeChangeHistoryController : BaseController
    {
        private readonly IBranchTypeChangeHistoryHandler _branchTypeChangeHistory;
        private readonly IReportGenerator _reportGenerator;
        public BranchTypeChangeHistoryController(
            IBranchTypeChangeHistoryHandler branchTypeChangeHistory,
            IReportGenerator reportGenerator)
        {
            _branchTypeChangeHistory = branchTypeChangeHistory;
            _branchTypeChangeHistory.NotNull(nameof(_branchTypeChangeHistory));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(BranchTypeChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto> branchTypeChangeHistory = await _branchTypeChangeHistory.Handle(inputDto, cancellationToken);
            return Ok(branchTypeChangeHistory);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, BranchTypeChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _branchTypeChangeHistory.Handle, CurrentUser, ReportLiterals.BranchTypeChangeHistory, connectionId);
            return Ok(inputDto);
        }
    }
}
