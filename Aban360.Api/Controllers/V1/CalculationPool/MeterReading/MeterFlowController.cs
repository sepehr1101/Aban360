using Aban360.CalculationPool.Application.Features.MeterReading.Services;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading
{
    [Route("v1/meter-flow")]
    public class MeterFlowController : BaseController
    {
        private readonly IMeterFlowService _meterFlowService;
        private readonly IMeterReadingFileRemovalService _meterReadingFileRemovalService;

        public MeterFlowController(
            IMeterFlowService meterFlowService,
            IMeterReadingFileRemovalService meterReadingFileRemovalService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
            _meterReadingFileRemovalService = meterReadingFileRemovalService;
            _meterReadingFileRemovalService.NotNull(nameof(meterReadingFileRemovalService));
        }

        [Obsolete]
        [HttpPost]
        [Route("initial-calculate/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingDetailDataOutputDto> result = await _meterFlowService.InitialCalculationAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("consumption-check/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingCheckedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckConsumption(int id, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto result = await _meterFlowService.ConsumptionCheckedAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Obsolete]
        [HttpPost]
        [Route("confirm/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmCalculation(int id, CancellationToken cancellationToken)
        {
            await _meterFlowService.CalculationConfirmationAsync(id, CurrentUser.UserId, cancellationToken);
            return Ok(id);
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

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFile(int id, CancellationToken cancellationToken)
        {
            await _meterReadingFileRemovalService.MeterReadingFileRemoveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
