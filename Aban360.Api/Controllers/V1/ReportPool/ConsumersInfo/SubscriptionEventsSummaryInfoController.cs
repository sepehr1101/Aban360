using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/subscription")]
    [ApiController]
    public class SubscriptionEventsSummaryInfoController : BaseController
    {
        private readonly ISubscriptionEventHandler _subscriptionEventHandler;
        public SubscriptionEventsSummaryInfoController(
            ISubscriptionEventHandler subscriptionEventHandler)
        {
            _subscriptionEventHandler = subscriptionEventHandler;
            _subscriptionEventHandler.NotNull(nameof(subscriptionEventHandler));
        }

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EventsSummaryOutputHeaderDto, EventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SearchInput searchInput)
        {
            ReportOutput<EventsSummaryOutputHeaderDto, EventsSummaryOutputDataDto> items = await _subscriptionEventHandler.Handle(searchInput.Input);
            return Ok(items);
        }
    }
}