using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.ReportPool
{
    [Route("v1/consumer")]
    public class IndividualSummeryInfoController : BaseController
    {
        private readonly IIndividualSummeryQueryService _individualSummeryQueryService;
        public IndividualSummeryInfoController(IIndividualSummeryQueryService individualSummeryQueryService)
        {
            _individualSummeryQueryService = individualSummeryQueryService;
            _individualSummeryQueryService.NotNull(nameof(individualSummeryQueryService));
        }

        [HttpGet, HttpPost]
        [Route("individual-ownership-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OwnerShipSummary(string billId)
        {
            IndividualSummaryDto summary = await _individualSummeryQueryService.GetOwnerShipSummery(billId, (short) IndividualEstateRelationEnum.OwnerShip);
            return Ok(summary);
        }
        
        [HttpGet, HttpPost]
        [Route("individual-stakeholders-summary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StakeHolderSummary(string billId)
        {
            IndividualSummaryDto summary = await _individualSummeryQueryService.GetStakeHolderSummery(billId,(short) IndividualEstateRelationEnum.OwnerShip);
            return Ok(summary);
        }
    }
}
