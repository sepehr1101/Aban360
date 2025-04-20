using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation")]
    public class TariffCalculationManager : BaseController
    {
        private readonly ITariffCalculationHandler _tariffCalculationHandler;
        public TariffCalculationManager(ITariffCalculationHandler tariffCalculationHandler)
        {
            _tariffCalculationHandler = tariffCalculationHandler;
            _tariffCalculationHandler.NotNull();
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody]TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffCalculationHandler.Test(tariffTestInput, cancellationToken);
            return Ok(result);
        }
    }
}
