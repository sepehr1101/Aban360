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
    [Route("v1/use-state-change-history")]
    public class UseStateChangeHistoryController : BaseController
    {
        private readonly IUseStateChangeHistoryHandler _useStateChangeHistory;
        private readonly IReportGenerator _reportGenerator;
        public UseStateChangeHistoryController(
            IUseStateChangeHistoryHandler useStateChangeHistory,
            IReportGenerator reportGenerator)
        {
            _useStateChangeHistory = useStateChangeHistory;
            _useStateChangeHistory.NotNull(nameof(_useStateChangeHistory));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateChangeHistoryHeaderOutputDto, UseStateChangeHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UseStateChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateChangeHistoryHeaderOutputDto, UseStateChangeHistoryDataOutputDto> useStateChangeHistory = await _useStateChangeHistory.Handle(inputDto, cancellationToken);
            return Ok(useStateChangeHistory);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UseStateChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _useStateChangeHistory.Handle, CurrentUser, ReportLiterals.UseStateChangeHistory, connectionId);
            return Ok(inputDto);
        }
    }
}
