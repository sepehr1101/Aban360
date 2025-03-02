using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("v1/network")]
    public class WaterMeterSummeryInfoController : BaseController
    {
        private readonly IWaterMeterSummeryQueryService _waterMeterSummeryQueryService;
        public WaterMeterSummeryInfoController(IWaterMeterSummeryQueryService waterMeterSummeryQueryService)
        {
            _waterMeterSummeryQueryService = waterMeterSummeryQueryService;
            _waterMeterSummeryQueryService.NotNull(nameof(waterMeterSummeryQueryService));
        }

        [HttpPost]
        [Route("water-meter-consumption")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<WaterMeterSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsumptionSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<WaterMeterSummaryDto> summary = await _waterMeterSummeryQueryService.GetInfo(searchInput.Input, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }

        [HttpPost]
        [Route("water-meter-witness")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<WaterMeterSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WitnessSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<WaterMeterSummaryDto> summary = await _waterMeterSummeryQueryService.GetInfo(searchInput.Input, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }
    }
}
