using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    public class NetworkController : BaseController
    {
        private readonly ISiphonSummeryQueryService _c0_siphonSummeryQueryService;
        private readonly IWaterMeterSummeryQueryService _c1_waterMeterSummeryQueryService;
        public NetworkController(ISiphonSummeryQueryService c0_siphonSummeryQueryService, IWaterMeterSummeryQueryService c1_waterMeterSummeryQueryService)
        {
            _c0_siphonSummeryQueryService = c0_siphonSummeryQueryService;
            _c0_siphonSummeryQueryService.NotNull(nameof(c0_siphonSummeryQueryService));
            _c1_waterMeterSummeryQueryService = c1_waterMeterSummeryQueryService;
            _c1_waterMeterSummeryQueryService.NotNull(nameof(c1_waterMeterSummeryQueryService));
        }

        [HttpPost]
        [Route("~/v1/network/siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SiphonSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput)
        {
            IEnumerable<SiphonSummaryDto> summary = await _c0_siphonSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }

        [HttpPost]
        [Route("~/v1/network/water-meter-consumption")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<WaterMeterSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsumptionSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<WaterMeterSummaryDto> summary = await _c1_waterMeterSummeryQueryService.GetInfo(searchInput.Input, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }

        [HttpPost]
        [Route("~/v1/network/water-meter-witness")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<WaterMeterSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WitnessSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<WaterMeterSummaryDto> summary = await _c1_waterMeterSummeryQueryService.GetInfo(searchInput.Input, (short)MeterUseTypeEnum.Consumption);
            return Ok(summary);
        }
    }
}
