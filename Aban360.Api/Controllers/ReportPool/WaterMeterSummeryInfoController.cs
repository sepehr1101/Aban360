using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumer")]
    public class WaterMeterSummeryInfoController : BaseController
    {
        private readonly IWaterMeterSummeryQueryService _waterMeterSummeryQueryService;
        public WaterMeterSummeryInfoController(IWaterMeterSummeryQueryService waterMeterSummeryQueryService)
        {
            _waterMeterSummeryQueryService = waterMeterSummeryQueryService;
            _waterMeterSummeryQueryService.NotNull(nameof(waterMeterSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("watermeter-consumption-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsumptionSummary(string billId)
        {
            WaterMeterSummaryDto summary = await _waterMeterSummeryQueryService.GetInfo(billId, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }
        
        [HttpGet, HttpPost]
        [Route("watermeter-witness-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WitnessSummary(string billId)
        {
            WaterMeterSummaryDto summary = await _waterMeterSummeryQueryService.GetInfo(billId, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }
    }
}
