using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITariffTestsService = global::Aban360.CalculationPool.Application.Features.TestTariff.Services.ITariffTestsService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation")]
    public class TariffBatchCalculationManager : BaseController
    {
        private readonly ITariffTestsService _tariffTestsService;
        public TariffBatchCalculationManager(ITariffTestsService tariffTestsService)
        {
            _tariffTestsService = tariffTestsService;
            _tariffTestsService.NotNull();
        }

        [HttpPost]
        [Route("test-batch")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody] CaluclationIntervalBatchTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            CaluclationIntervalDiscrepancytWrapper wrapper = await _tariffTestsService.TariffTestBatchAsync(tariffTestInput, cancellationToken);
            return Ok(wrapper);
        }
    }
}
