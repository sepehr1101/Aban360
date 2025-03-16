using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceLineItemInsertModeGetAllHandler : IInvoiceLineItemInsertModeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        public InvoiceLineItemInsertModeGetAllHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));
        }

        public async Task<ICollection<InvoiceLineItemInsertModeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<InvoiceLineItemInsertMode> invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get();
            return _mapper.Map<ICollection<InvoiceLineItemInsertModeGetDto>>(invoiceLineItemInsertMode);
        }
    }
}
