using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class InvoiceLineItemInsertModeDeleteHandler : IInvoiceLineItemInsertModeDeleteHandler
    {
        private readonly IInvoiceLineItemInsertModeCommandService _invoiceLineItemInsertModeCommandService;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        public InvoiceLineItemInsertModeDeleteHandler(
            IInvoiceLineItemInsertModeCommandService invoiceLineItemInsertModeCommandService,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService)
        {
            _invoiceLineItemInsertModeCommandService = invoiceLineItemInsertModeCommandService;
            _invoiceLineItemInsertModeCommandService.NotNull(nameof(invoiceLineItemInsertModeCommandService));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));
        }

        public async Task Handle(InvoiceLineItemInsertModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(deleteDto.Id);
            if (invoiceLineItemInsertMode == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceLineItemInsertModeCommandService.Remove(invoiceLineItemInsertMode);
        }
    }
}
