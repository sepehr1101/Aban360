using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumer")]
    public class EstateSummeryInfoController : BaseController
    {
        private readonly IEstateSummeryQueryService _estateSummeryQueryService;
        public EstateSummeryInfoController(IEstateSummeryQueryService estateSummeryQueryService)
        {
            _estateSummeryQueryService = estateSummeryQueryService;
            _estateSummeryQueryService.NotNull(nameof(estateSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("estate-summary/{biilId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ResultEstateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string biilId)
        {
            ResultEstateDto summary = await _estateSummeryQueryService.GetSummery(biilId);
            return Ok(summary);
        }
    }
}
