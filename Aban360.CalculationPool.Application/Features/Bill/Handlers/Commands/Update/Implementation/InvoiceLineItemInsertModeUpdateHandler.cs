using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class InvoiceLineItemInsertModeUpdateHandler : IInvoiceLineItemInsertModeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        public InvoiceLineItemInsertModeUpdateHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));
        }

        public async Task Handle(InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceLineItemInsertMode);
        }
    }
}
