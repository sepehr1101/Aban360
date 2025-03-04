using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceLineItemInsertModeGetSingleHandler : IInvoiceLineItemInsertModeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        public InvoiceLineItemInsertModeGetSingleHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));
        }

        public async Task<InvoiceLineItemInsertModeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(id);
            if (invoiceLineItemInsertMode == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<InvoiceLineItemInsertModeGetDto>(invoiceLineItemInsertMode);
        }
    }
}
