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
    [Route("v1/usage-change-history")]
    public class UsageChangeHistoryController : BaseController
    {
        private readonly IUsageChangeHistoryHandler _usageChangeHistory;
        private readonly IReportGenerator _reportGenerator;
        public UsageChangeHistoryController(
            IUsageChangeHistoryHandler usageChangeHistory,
            IReportGenerator reportGenerator)
        {
            _usageChangeHistory = usageChangeHistory;
            _usageChangeHistory.NotNull(nameof(_usageChangeHistory));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageChangeHistoryHeaderOutputDto, UsageChangeHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageChangeHistoryHeaderOutputDto, UsageChangeHistoryDataOutputDto> usageChangeHistory = await _usageChangeHistory.Handle(inputDto, cancellationToken);
            return Ok(usageChangeHistory);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UsageChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _usageChangeHistory.Handle, CurrentUser, ReportLiterals.UsageChangeHistory, connectionId);
            return Ok(inputDto);
        }
    }
}
