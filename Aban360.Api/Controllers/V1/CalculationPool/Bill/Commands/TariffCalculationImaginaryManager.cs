using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation_imaginary")]
    public class TariffCalculationImaginaryManager : BaseController
    {
        private readonly ITariffCalculationImaginaryHandler _tariffCalculationImaginaryHandler;
        public TariffCalculationImaginaryManager(ITariffCalculationImaginaryHandler tariffCalculationImaginaryHandler)
        {
            _tariffCalculationImaginaryHandler = tariffCalculationImaginaryHandler;
            _tariffCalculationImaginaryHandler.NotNull();
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IntervalCalculationResultWrapper>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Test([FromBody] IntervalBillSubscriptionInfoImaginary tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffCalculationImaginaryHandler.Test(tariffTestInput, cancellationToken);
            return Ok(result);
        }
    }
}
