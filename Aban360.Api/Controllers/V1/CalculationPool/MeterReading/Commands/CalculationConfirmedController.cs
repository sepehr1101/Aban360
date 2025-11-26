using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/calculation-confirmed")]
    public class CalculationConfirmedController : BaseController
    {
        private readonly ICalculationConfirmedHandler _calculationConfirmedHandler;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        public CalculationConfirmedController(
            ICalculationConfirmedHandler calculationConfirmedHandler, 
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _calculationConfirmedHandler = calculationConfirmedHandler;
            _calculationConfirmedHandler.NotNull(nameof(calculationConfirmedHandler));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        [HttpPost]
        [Route("confirmed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculationConfirmed(int id, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(id, cancellationToken);
            await _calculationConfirmedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
