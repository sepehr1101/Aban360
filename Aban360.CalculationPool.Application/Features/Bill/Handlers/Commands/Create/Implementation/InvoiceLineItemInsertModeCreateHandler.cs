using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceLineItemInsertModeCreateHandler : IInvoiceLineItemInsertModeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeCommandService _invoiceLineItemInsertModeCommandService;
        public InvoiceLineItemInsertModeCreateHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeCommandService invoiceLineItemInsertModeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeCommandService = invoiceLineItemInsertModeCommandService;
            _invoiceLineItemInsertModeCommandService.NotNull(nameof(invoiceLineItemInsertModeCommandService));
        }

        public async Task Handle(InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken)
        {
            var invoiceLineItemInsertMode = _mapper.Map<InvoiceLineItemInsertMode>(createDto);
            if (invoiceLineItemInsertMode == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceLineItemInsertModeCommandService.Add(invoiceLineItemInsertMode);
        }
    }
}
