using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/summary")]
    public class ConsumerSummaryInfoController : BaseController
    {
        private readonly IConsumerSummaryInfo _summery;
        public ConsumerSummaryInfoController(IConsumerSummaryInfo summery)
        {
            _summery = summery;
            _summery.NotNull(nameof(ConsumerSummaryInfo));
        }

        [HttpGet, HttpPost]
        [Route("consumer-info/{biilId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ResultSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string biilId)
        {
            ResultSummaryDto summary = await _summery.GetSummery(biilId);
            return Ok(summary);
        }
    }
}
