using Aban360.CalculationPool.Domain.Features.MeterChange.Dto;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/meter")]
    public class MeterChangeSetController : BaseController
    {
        public MeterChangeSetController()
        {
        }

        [HttpPost]
        [Route("set-change")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterChangeSetInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetChange(MeterChangeSetInputDto input, CancellationToken cancellationToken)
        {
            //Set MeterChange
            return Ok(input);
        }
    }
}
