using Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation-imaginary")]
    public class TariffCalculationImaginaryManager : BaseController
    {
        private readonly ITariffTestImaginaryCustomerHandler _tariffCalculationImaginaryHandler;
        public TariffCalculationImaginaryManager(ITariffTestImaginaryCustomerHandler tariffCalculationImaginaryHandler)
        {
            _tariffCalculationImaginaryHandler = tariffCalculationImaginaryHandler;
            _tariffCalculationImaginaryHandler.NotNull();
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IntervalCalculationResultWrapper>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Test([FromBody] TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffCalculationImaginaryHandler.Handle(tariffTestInput, cancellationToken);
            return Ok(result);
        }
    }
}
