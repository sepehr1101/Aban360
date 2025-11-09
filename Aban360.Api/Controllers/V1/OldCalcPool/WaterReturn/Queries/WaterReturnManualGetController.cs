using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return-manual")]
    public class WaterReturnManualGetController : BaseController
    {
        private readonly IRepairGetHandler _repairHandler;
        public WaterReturnManualGetController(IRepairGetHandler repairHandler)
        {
            _repairHandler = repairHandler;
            _repairHandler.NotNull(nameof(repairHandler));
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RepairGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            RepairGetDto result = await _repairHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
