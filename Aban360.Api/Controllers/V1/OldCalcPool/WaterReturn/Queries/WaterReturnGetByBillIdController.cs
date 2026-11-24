using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using IBillReturnRepairService = global::Aban360.OldCalcPool.Application.Features.WaterReturn.Services.IBillReturnRepairService;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return")]
    public class WaterReturnGetByBillIdController : BaseController
    {
        private readonly IBillReturnRepairService _billReturnRepairService;
        public WaterReturnGetByBillIdController(IBillReturnRepairService billReturnRepairService)
        {
            _billReturnRepairService = billReturnRepairService;
            _billReturnRepairService.NotNull(nameof(billReturnRepairService));
        }

        [HttpPost]
        [Route("get-by-billid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<RepairGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            IEnumerable<RepairGetDto> result = await _billReturnRepairService.RepairGetByBillIdAsync(input, cancellationToken);
            return Ok(result);
        }
    }
}
