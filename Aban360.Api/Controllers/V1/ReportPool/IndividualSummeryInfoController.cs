using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dto;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("v1/individual")]
    public class IndividualSummeryInfoController : BaseController
    {
        private readonly IIndividualSummeryQueryService _individualSummeryQueryService;
        public IndividualSummeryInfoController(IIndividualSummeryQueryService individualSummeryQueryService)
        {
            _individualSummeryQueryService = individualSummeryQueryService;
            _individualSummeryQueryService.NotNull(nameof(individualSummeryQueryService));
        }

        [HttpPost]
        [Route("owner")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OwnerShipSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<IndividualSummaryDto> summary = await _individualSummeryQueryService.GetOwnerShipSummery(searchInput.Input, (short)IndividualEstateRelationEnum.OwnerShip);
            return Ok(summary);
        }

        [HttpPost]
        [Route("stakeholder")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StakeHolderSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<IndividualSummaryDto> summary = await _individualSummeryQueryService.GetStakeHolderSummery(searchInput.Input, (short)IndividualEstateRelationEnum.OwnerShip);
            return Ok(summary);
        }
    }
}
