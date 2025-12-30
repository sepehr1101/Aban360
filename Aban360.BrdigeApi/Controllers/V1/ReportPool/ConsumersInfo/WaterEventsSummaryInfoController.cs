using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/water")]
    public class WaterEventsSummaryInfoController : BaseController
    {
        private readonly ISubscriptionEventHandler _subscriptionEventHandler;
        public WaterEventsSummaryInfoController(
            ISubscriptionEventHandler subscriptionEventHandler)
        {
            _subscriptionEventHandler = subscriptionEventHandler;
            _subscriptionEventHandler.NotNull(nameof(subscriptionEventHandler));
        }

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] CardexInput searchInput)
        {
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> items = await _subscriptionEventHandler.Handle(searchInput.Input, searchInput.FromDateJalali);
            return Ok(items);
        }
    }
}
