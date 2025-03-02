using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class InvoiceTypeDeleteHandler : IInvoiceTypeDeleteHandler
    {
        private readonly IInvoiceTypeCommandService _invoiceTypeCommandService;
        private readonly IInvoiceTypeQueryService _invoiceTypeQueryService;
        public InvoiceTypeDeleteHandler(
            IInvoiceTypeCommandService invoiceTypeCommandService,
            IInvoiceTypeQueryService invoiceTypeQueryService)
        {
            _invoiceTypeCommandService = invoiceTypeCommandService;
            _invoiceTypeCommandService.NotNull(nameof(invoiceTypeCommandService));

            _invoiceTypeQueryService = invoiceTypeQueryService;
            _invoiceTypeQueryService.NotNull(nameof(invoiceTypeQueryService));
        }

        public async Task Handle(InvoiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var invoiceType = await _invoiceTypeQueryService.Get(deleteDto.Id);
            if (invoiceType == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceTypeCommandService.Remove(invoiceType);
        }
    }
}
