using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/network")]
    public class SiphonSummeryInfoController : BaseController
    {
        private readonly ISiphonSummeryQueryService _siphonSummeryQueryService;
        public SiphonSummeryInfoController(ISiphonSummeryQueryService siphonSummeryQueryService)
        {
            _siphonSummeryQueryService = siphonSummeryQueryService;
            _siphonSummeryQueryService.NotNull(nameof(siphonSummeryQueryService));
        }

        [HttpPost]
        [Route("siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SiphonSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput)
        {
            IEnumerable<SiphonSummaryDto> summary = await _siphonSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }
    }
}
