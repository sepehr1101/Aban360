using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceStatusGetAllHandler : IInvoiceStatusGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusQueryService _invoiceStatusQueryService;
        public InvoiceStatusGetAllHandler(
            IMapper mapper,
            IInvoiceStatusQueryService invoiceStatusQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusQueryService = invoiceStatusQueryService;
            _invoiceStatusQueryService.NotNull(nameof(invoiceStatusQueryService));
        }

        public async Task<ICollection<InvoiceStatusGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<InvoiceStatus> invoiceStatus = await _invoiceStatusQueryService.Get();
            return _mapper.Map<ICollection<InvoiceStatusGetDto>>(invoiceStatus);
        }
    }
}
