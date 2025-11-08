using Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.WaterReturn.Commands
{
    [Route("v1/water-return-manual")]
    public class WaterReturnManualCreateController : BaseController
    {
        private readonly IRepairCreateHandler _repairHandler;
        public WaterReturnManualCreateController(IRepairCreateHandler repairHandler)
        {
            _repairHandler = repairHandler;
            _repairHandler.NotNull(nameof(repairHandler));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingToCreateRepairDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingToCreateRepairDto input, CancellationToken cancellationToken)
        {
            await _repairHandler.Handle(input, cancellationToken);
            return Ok(input);
        }
    }
}
