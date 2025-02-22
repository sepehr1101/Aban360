using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumer")]
    public class SiphonSummeryInfoController : BaseController
    {
        private readonly ISiphonSummeryQueryService _siphonSummeryQueryService;
        public SiphonSummeryInfoController(ISiphonSummeryQueryService siphonSummeryQueryService)
        {
            _siphonSummeryQueryService = siphonSummeryQueryService;
            _siphonSummeryQueryService.NotNull(nameof(siphonSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("siphon-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId)
        {
            SiphonSummaryDto summary = await _siphonSummeryQueryService.GetInfo(billId);
            return Ok(summary);
        }
    }
}
