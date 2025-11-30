using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Obsolete]
    [Route("v1/meter-flow")]
    public class MeterFlowCalculationConfirmationController : BaseController
    {
        private readonly ICalculationConfirmationHandler _calculationConfirmedHandler;
        public MeterFlowCalculationConfirmationController(
            ICalculationConfirmationHandler calculationConfirmedHandler, 
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _calculationConfirmedHandler = calculationConfirmedHandler;
            _calculationConfirmedHandler.NotNull(nameof(calculationConfirmedHandler));
        }

        [HttpPost]
        [Route("confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmCalculation(int id, CancellationToken cancellationToken)
        {           
            await _calculationConfirmedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
