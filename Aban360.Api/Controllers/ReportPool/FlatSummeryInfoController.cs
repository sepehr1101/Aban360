using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumer")]
    public class FlatSummeryInfoController : BaseController
    {
        private readonly IFlatSummeryQueryService _flatSummeryQueryService;
        public FlatSummeryInfoController(IFlatSummeryQueryService flatSummeryQueryService)
        {
            _flatSummeryQueryService = flatSummeryQueryService;
            _flatSummeryQueryService.NotNull(nameof(flatSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("flat-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ResultFlatDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId)
        {
            ResultFlatDto summary = await _flatSummeryQueryService.GetInfo(billId);
            return Ok(summary);
        }
    }
}
