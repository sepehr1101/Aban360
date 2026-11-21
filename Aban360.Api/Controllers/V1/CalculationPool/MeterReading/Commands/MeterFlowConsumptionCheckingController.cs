using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowConsumptionCheckingController : BaseController
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowConsumptionCheckingController(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        [HttpPost]
        [Route("consumption-check/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckConsumption(int id, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto result = await _meterFlowService.ConsumptionCheckedAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
