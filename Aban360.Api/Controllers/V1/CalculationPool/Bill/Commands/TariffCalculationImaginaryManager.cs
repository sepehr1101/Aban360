using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITariffTestsService = global::Aban360.CalculationPool.Application.Features.TestTariff.Services.ITariffTestsService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation-imaginary")]
    public class TariffCalculationImaginaryManager : BaseController
    {
        private readonly ITariffTestsService _tariffTestsService;
        public TariffCalculationImaginaryManager(ITariffTestsService tariffTestsService)
        {
            _tariffTestsService = tariffTestsService;
            _tariffTestsService.NotNull();
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IntervalCalculationResultWrapper>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Test([FromBody] TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffTestsService.TariffTestImaginaryCustomerAsync(tariffTestInput, cancellationToken);
            return Ok(result);
        }
    }
}
