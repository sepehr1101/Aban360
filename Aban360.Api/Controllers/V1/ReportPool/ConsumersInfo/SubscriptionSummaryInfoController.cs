using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/subscription")]
    public class SubscriptionSummaryInfoController : BaseController
    {
        private readonly ISubscriptionSummaryInfoGetHandler _consumerSummeryHandler;
        public SubscriptionSummaryInfoController(ISubscriptionSummaryInfoGetHandler consumerSummaryHandler)
        {
            _consumerSummeryHandler = consumerSummaryHandler;
            _consumerSummeryHandler.NotNull(nameof(_consumerSummeryHandler));
        }

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput,CancellationToken cancellation)
        {
            ConsumerSummaryDto summary = await _consumerSummeryHandler.Handle(searchInput.Input,cancellation);
            return Ok(summary);
        }
    }
}
