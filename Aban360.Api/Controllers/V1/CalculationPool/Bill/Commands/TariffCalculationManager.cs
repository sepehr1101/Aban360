using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITariffTestsService = global::Aban360.CalculationPool.Application.Features.TestTariff.Services.ITariffTestsService;
using IInvoiceService = global::Aban360.CalculationPool.Application.Features.Bill.Services.IInvoiceService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation")]
    public class TariffCalculationManager : BaseController
    {
        private readonly ITariffTestsService _tariffTestsService;
        private readonly IInvoiceService _invoiceService;
        private readonly IUnitOfWork _uow;
        public TariffCalculationManager(ITariffTestsService tariffTestsService, IInvoiceService invoiceService, IUnitOfWork uow)
        {
            _tariffTestsService = tariffTestsService;
            _tariffTestsService.NotNull();
            _invoiceService = invoiceService;
            _invoiceService.NotNull(nameof(invoiceService));
            _uow = uow;
            _uow.NotNull(nameof(uow));
        }

        [HttpPost]
        [Route("test-basic")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody] TariffTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            var result = await _tariffTestsService.TariffTestSingleCustomerAsync(tariffTestInput, cancellationToken);
            await _invoiceService.InvoiceInserterAsync(result, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(result);
        }
    }
}
