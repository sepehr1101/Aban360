using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/consumption-checked")]
    public class ConsumptionCheckedController : BaseController
    {
        private readonly IConsumptionCheckedHandler _consumptionControlHandler;
        public ConsumptionCheckedController(IConsumptionCheckedHandler consumptionControlHandler)
        {
            _consumptionControlHandler = consumptionControlHandler;
            _consumptionControlHandler.NotNull(nameof(consumptionControlHandler));
        }

        [HttpPost]
        [Route("checked")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailCheckedDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsumptionChecked(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingDetailCheckedDto> result = await _consumptionControlHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
