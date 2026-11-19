using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Obsolete]
    [Route("v1/meter-flow")]
    public class MeterFlowCalculationConfirmationController : BaseController
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowCalculationConfirmationController(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        [HttpPost]
        [Route("confirm/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmCalculation(int id, CancellationToken cancellationToken)
        {
            await _meterFlowService.CalculationConfirmationAsync(id, CurrentUser.UserId, cancellationToken);
            return Ok(id);
        }
    }
}
