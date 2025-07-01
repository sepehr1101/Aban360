using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/latest-water-meter")]
    public class LatestWaterMeterInfoController : BaseController
    {
        private readonly ILatestWaterMeterInfoGetHandler _latestWaterMetersSummaryInfoGetHandler;
        public LatestWaterMeterInfoController(ILatestWaterMeterInfoGetHandler latestWaterMetersSummaryInfoGetHandler)
        {
            _latestWaterMetersSummaryInfoGetHandler = latestWaterMetersSummaryInfoGetHandler;
            _latestWaterMetersSummaryInfoGetHandler.NotNull(nameof(latestWaterMetersSummaryInfoGetHandler));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestWaterMeterInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            LatestWaterMeterInfoDto summary = await _latestWaterMetersSummaryInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
