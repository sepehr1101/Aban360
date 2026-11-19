using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowAmountConfirmedController : BaseController
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowAmountConfirmedController(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        [HttpPost]
        [Route("amount-confirmed/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmount(int id, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto result = await _meterFlowService.CalculationConfirmationAsync(id, CurrentUser.UserId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("amount-confirmed-then-send-sms/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmountThenSendSms(int id, CancellationToken cancellationToken)
        {
            await _meterFlowService.MeterReadingCalculationConfirmThenSendSmsAsync(id, CurrentUser.UserId, cancellationToken);
            return Ok(id);
        }
    }
}
