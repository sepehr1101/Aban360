using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/amount-checked")]
    public class AmountCheckedController : BaseController
    {
        private readonly IAmountCheckedHandler _amountCheckedHandler;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        public AmountCheckedController(
            IAmountCheckedHandler amountCheckedHandler, 
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _amountCheckedHandler = amountCheckedHandler;
            _amountCheckedHandler.NotNull(nameof(amountCheckedHandler));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        [HttpPost]
        [Route("checked")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailCheckedDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AmountChecked(int id, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(id, cancellationToken);
            IEnumerable<MeterReadingDetailCheckedDto> result = await _amountCheckedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
