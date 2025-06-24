using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/subscription")]
    [ApiController]
    public class SubscriptionEventsSummaryInfoController : BaseController
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        public SubscriptionEventsSummaryInfoController(
            ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));
        }

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EventsSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SearchInput searchInput)
        {
            IEnumerable<EventsSummaryDto> items = await _subscriptionEventQueryService.GetEventsSummaryDtos(searchInput.Input);
            return Ok(items);
        }
    }
}