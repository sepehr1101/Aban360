using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
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

        public async Task<InvoiceLineItemInsertModeGetDto> Handle(InvoiceLineItemInsertModeEnum id, CancellationToken cancellationToken)
        {
            InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(id);
            return _mapper.Map<InvoiceLineItemInsertModeGetDto>(invoiceLineItemInsertMode);
        }
    }
}
