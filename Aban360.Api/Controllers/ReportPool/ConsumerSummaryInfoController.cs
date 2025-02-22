using Aban360.Api.Controllers.V1;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/summery")]
    public class ConsumerSummaryInfoController:BaseController
    {
        private readonly IConsumerSummaryInfo _summery;
        public ConsumerSummaryInfoController(IConsumerSummaryInfo summery)
        {
            _summery = summery;
            _summery.NotNull(nameof(ConsumerSummaryInfo));
        }

        [HttpGet, HttpPost]
        [Route("summery/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var summary=await _summery.GetSummery(id);
            return Ok(summary);
        }
    }
}
