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
        private readonly IInvoiceInserterHandler _invoiceInserterHandler;
        public TariffCalculationManager(
            ITariffCalculationHandler tariffCalculationHandler, 
            IInvoiceInserterHandler invoiceInserterHandler)
        {
            _tariffCalculationHandler = tariffCalculationHandler;
            _tariffCalculationHandler.NotNull();

            _invoiceInserterHandler = invoiceInserterHandler;
            _invoiceInserterHandler.NotNull(nameof(invoiceInserterHandler));
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody]TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffCalculationHandler.Test(tariffTestInput, cancellationToken);
            _invoiceInserterHandler.Handle(result,cancellationToken );
            return Ok(result);
        }
    }
}
