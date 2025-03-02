using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class InvoiceLineItemInsertModeGetAllHandler : IInvoiceLineItemInsertModeGetAllHandler
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
            var invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get();
            if (invoiceLineItemInsertMode == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<InvoiceLineItemInsertModeGetDto>>(invoiceLineItemInsertMode);
        }
    }
}
