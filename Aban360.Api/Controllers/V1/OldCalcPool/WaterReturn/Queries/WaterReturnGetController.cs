using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using IBillReturnRepairService = global::Aban360.OldCalcPool.Application.Features.WaterReturn.Services.IBillReturnRepairService;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return")]
    public class WaterReturnGetController : BaseController
    {
        private readonly IBillReturnRepairService _billReturnRepairService;
        public WaterReturnGetController(IBillReturnRepairService billReturnRepairService)
        {
            _billReturnRepairService = billReturnRepairService;
            _billReturnRepairService.NotNull(nameof(billReturnRepairService));
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RepairGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            RepairGetDto result = await _billReturnRepairService.RepairGetAsync(input, cancellationToken);
            return Ok(result);
        }
    }
}
