using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("v1/realm")]
    public class RealmFlatSummeryInfoController : BaseController
    {
        private readonly IFlatSummeryQueryService _flatSummeryQueryService;
        public RealmFlatSummeryInfoController(IFlatSummeryQueryService flatSummeryQueryService)
        {
            _flatSummeryQueryService = flatSummeryQueryService;
            _flatSummeryQueryService.NotNull(nameof(flatSummeryQueryService));
        }

        [HttpPost]
        [Route("flat")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ResultFlatDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput searchInput)
        {
            IEnumerable<ResultFlatDto> summary = await _flatSummeryQueryService.GetInfo(searchInput.Input);
            return Ok(summary);
        }
    }
}
