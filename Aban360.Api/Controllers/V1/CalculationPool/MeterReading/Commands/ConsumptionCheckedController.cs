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
        private readonly IConsumptionCheckedHandler _consumptionCheckedHandler;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        public ConsumptionCheckedController(
            IConsumptionCheckedHandler consumptionCheckedHandler, 
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _consumptionCheckedHandler = consumptionCheckedHandler;
            _consumptionCheckedHandler.NotNull(nameof(consumptionCheckedHandler));
           
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        [HttpPost]
        [Route("checked")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailCheckedDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsumptionChecked(int id, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(id, cancellationToken);
            IEnumerable<MeterReadingDetailCheckedDto> result = await _consumptionCheckedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
