using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
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
        public async Task<IActionResult> GetBranchEventsSummary([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<BranchEventsDto> eventsBranchsDtos = await _customerBranchEventHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(eventsBranchsDtos);
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        //[BillIdFromSearchInputAuthorization]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            CardexInput input = new(searchInput.Input,null);
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> eventsBranchsDtos = await _branchEventSummaryHandler.Handle(input, cancellationToken);
            return Ok(eventsBranchsDtos);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 231;
            CardexInput input = new(searchInput.Input, null);
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> calculationDetails = await _branchEventSummaryHandler.Handle(input, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }


        //with-last-db

        [HttpPost]
        [Route("summary-lastdb")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBranchSummaryInfo_LastDb([FromBody] CardexInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> items = await _branchEventSummaryHandler.HandleWithLastDb(searchInput, cancellationToken);
            return Ok(items);
        }


        [HttpPost]
        [Route("sti-lastdb")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport_LastDb([FromBody] CardexInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 230;
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> calculationDetails = await _branchEventSummaryHandler.HandleWithLastDb(searchInput, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}