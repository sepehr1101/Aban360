using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/branch-events")]
    public class BranchEventsSummaryController : BaseController
    {
        private readonly ICustomerBranchEventHandler _customerBranchEventHandler;
        private readonly IBranchEventSummaryHandler _branchEventSummaryHandler;
        public BranchEventsSummaryController(
            ICustomerBranchEventHandler customerBranchEventHandler,
            IBranchEventSummaryHandler branchEventSummaryHandler)
        {
            _customerBranchEventHandler = customerBranchEventHandler;
            _customerBranchEventHandler.NotNull(nameof(customerBranchEventHandler));

            _branchEventSummaryHandler = branchEventSummaryHandler;
            _branchEventSummaryHandler.NotNull(nameof(branchEventSummaryHandler));
        }

        [HttpPost]
        [Route("summary-beta")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BranchEventsDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchEventsSummary([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<BranchEventsDto> eventsBranchsDtos = await _customerBranchEventHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(eventsBranchsDtos);
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> eventsBranchsDtos = await _branchEventSummaryHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(eventsBranchsDtos);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 231ُ;
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> calculationDetails = await _branchEventSummaryHandler.Handle(searchInput.Input, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}