using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/realm")]
    public class RealmEstateSummeryInfoController : BaseController
    {
        private readonly IEstateSummeryQueryService _estateSummeryQueryService;
        public RealmEstateSummeryInfoController(IEstateSummeryQueryService estateSummeryQueryService)
        {
            _estateSummeryQueryService = estateSummeryQueryService;
            _estateSummeryQueryService.NotNull(nameof(estateSummeryQueryService));
        }

        [HttpPost]
        [Route("estate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ResultEstateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput)
        {
            IEnumerable<ResultEstateDto> summary = await _estateSummeryQueryService.GetSummery(searchInput.Input);
            return Ok(summary);
        }
    }
}
