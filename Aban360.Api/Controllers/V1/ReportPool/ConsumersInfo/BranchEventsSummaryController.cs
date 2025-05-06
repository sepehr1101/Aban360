using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/branch-events")]
    public class BranchEventsSummaryController: BaseController
    {
        private readonly ICustomerBranchEventHandler _customerBranchEventHandler;
        public BranchEventsSummaryController(ICustomerBranchEventHandler customerBranchEventHandler)
        {
            _customerBranchEventHandler = customerBranchEventHandler;
            _customerBranchEventHandler.NotNull(nameof(customerBranchEventHandler));
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BranchEventsDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchEventsSummary([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<BranchEventsDto> eventsBranchsDtos = await _customerBranchEventHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(eventsBranchsDtos);
        }
    }
}