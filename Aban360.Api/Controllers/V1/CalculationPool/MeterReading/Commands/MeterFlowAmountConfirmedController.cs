using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
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
        private readonly IMeterReadingCalculationConfirmThenSendSmsHandler _confirmationThenSendSmsHandler;
        public MeterFlowAmountConfirmedController(
            ICalculationConfirmationHandler confirmationHandler,
            IMeterReadingCalculationConfirmThenSendSmsHandler confirmationThenSendSmsHandler)
        {
            _confirmationHandler = confirmationHandler;
            _confirmationHandler.NotNull(nameof(confirmationHandler));

            _confirmationThenSendSmsHandler = confirmationThenSendSmsHandler;
            _confirmationThenSendSmsHandler.NotNull(nameof(confirmationThenSendSmsHandler));
        }

        [HttpPost]
        [Route("amount-confirmed/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmount(int id, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto result = await _confirmationHandler.Handle(id, CurrentUser.UserId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("amount-confirmed-then-send-sms/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmountThenSendSms(int id, CancellationToken cancellationToken)
        {
             await _confirmationThenSendSmsHandler.Handle(id, CurrentUser.UserId, cancellationToken);
            return Ok(id);
        }
    }
}
