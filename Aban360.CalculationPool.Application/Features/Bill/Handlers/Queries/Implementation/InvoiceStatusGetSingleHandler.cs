using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceStatusGetSingleHandler : IInvoiceStatusGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusQueryService _invoiceStatusQueryService;
        public InvoiceStatusGetSingleHandler(
            IMapper mapper,
            IInvoiceStatusQueryService invoiceStatusQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusQueryService = invoiceStatusQueryService;
            _invoiceStatusQueryService.NotNull(nameof(invoiceStatusQueryService));
        }

        public async Task<InvoiceStatusGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            InvoiceStatus invoiceStatus = await _invoiceStatusQueryService.Get(id);
            return _mapper.Map<InvoiceStatusGetDto>(invoiceStatus);
        }
    }
}
