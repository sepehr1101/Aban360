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
        private readonly IAmountCheckedHandler _amountCheckedHandler;
        public MeterFlowAmountConfirmedController(
            IAmountCheckedHandler amountCheckedHandler)
        {
            _amountCheckedHandler = amountCheckedHandler;
            _amountCheckedHandler.NotNull(nameof(amountCheckedHandler));
        }

        [HttpPost]
        [Route("amount-confirmed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAmount(int id, CancellationToken cancellationToken)
        {          
            await _amountCheckedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
