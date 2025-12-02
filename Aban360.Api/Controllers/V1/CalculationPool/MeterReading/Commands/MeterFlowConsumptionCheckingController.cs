using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowConsumptionCheckingController : BaseController
    {
        private readonly IConsumptionCheckedHandler _consumptionCheckedHandler;
        public MeterFlowConsumptionCheckingController(
            IConsumptionCheckedHandler consumptionCheckedHandler,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _consumptionCheckedHandler = consumptionCheckedHandler;
            _consumptionCheckedHandler.NotNull(nameof(consumptionCheckedHandler));
        }

        [HttpPost]
        [Route("consumption-check/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckConsumption(int id, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto result = await _consumptionCheckedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
