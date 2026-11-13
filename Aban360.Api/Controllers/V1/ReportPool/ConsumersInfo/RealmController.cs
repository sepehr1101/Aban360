using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    public class RealmController : BaseController
    {
        private readonly IEstateSummeryQueryService _c0_estateSummeryQueryService;
        private readonly IFlatSummeryQueryService _c1_flatSummeryQueryService;
        public RealmController(IEstateSummeryQueryService c0_estateSummeryQueryService, IFlatSummeryQueryService c1_flatSummeryQueryService)
        {
            _c0_estateSummeryQueryService = c0_estateSummeryQueryService;
            _c0_estateSummeryQueryService.NotNull(nameof(c0_estateSummeryQueryService));
            _c1_flatSummeryQueryService = c1_flatSummeryQueryService;
            _c1_flatSummeryQueryService.NotNull(nameof(c1_flatSummeryQueryService));
        }

        [HttpPost]
        [Route("~/v1/realm/estate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ResultEstateDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromRealmEstateSummeryInfo([FromBody] SearchInput searchInput)
        {
            IEnumerable<ResultEstateDto> summary = await _c0_estateSummeryQueryService.GetSummery(searchInput.Input);
            return Ok(summary);
        }

        [HttpPost]
        [Route("~/v1/realm/flat")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ResultFlatDto>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromRealmFlatSummeryInfo([FromBody] SearchInput searchInput)
        {
            IEnumerable<ResultFlatDto> summary = await _c1_flatSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }
    }
}
