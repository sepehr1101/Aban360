using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumerInfo")]
    public class ConsumerSummaryInfoController : BaseController
    {
        private readonly IConsumerSummaryQueryService _consumerSummeryQueryService;
        public ConsumerSummaryInfoController(IConsumerSummaryQueryService summery)
        {
            _consumerSummeryQueryService = summery;
            _consumerSummeryQueryService.NotNull(nameof(_consumerSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string biilId)
        {
            ConsumerSummaryDto summary = await _consumerSummeryQueryService.GetInfo(biilId);
            return Ok(summary);
        }
    }
}
