using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class InvoiceStatusDeleteHandler : IInvoiceStatusDeleteHandler
    {
        private readonly IInvoiceStatusCommandService _invoiceStatusCommandService;
        private readonly IInvoiceStatusQueryService _invoiceStatusQueryService;
        public InvoiceStatusDeleteHandler(
            IInvoiceStatusCommandService invoiceStatusCommandService,
            IInvoiceStatusQueryService invoiceStatusQueryService)
        {
            _invoiceStatusCommandService = invoiceStatusCommandService;
            _invoiceStatusCommandService.NotNull(nameof(invoiceStatusCommandService));

            _invoiceStatusQueryService = invoiceStatusQueryService;
            _invoiceStatusQueryService.NotNull(nameof(invoiceStatusQueryService));
        }

        public async Task Handle(InvoiceStatusDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var invoiceStatus = await _invoiceStatusQueryService.Get(deleteDto.Id);
            if (invoiceStatus == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceStatusCommandService.Remove(invoiceStatus);
        }
    }
}
