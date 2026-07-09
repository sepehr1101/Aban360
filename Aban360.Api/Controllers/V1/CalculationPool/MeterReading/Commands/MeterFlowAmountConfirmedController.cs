using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowAmountConfirmedController : BaseController
    {
        private readonly ICalculationConfirmationHandler _confirmationHandler;
        public MeterFlowAmountConfirmedController(ICalculationConfirmationHandler confirmationHandler)
        {
            _confirmationHandler = confirmationHandler;
            _confirmationHandler.NotNull(nameof(confirmationHandler));
        }

        [HttpPost]
        [Route("amount-confirmed/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmount(int id, CancellationToken cancellationToken)
        {           
            MeterReadingCheckedOutputDto result = await _confirmationHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
