using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceTypeGetAllHandler : IInvoiceTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeQueryService _invoiceTypeQueryService;
        public InvoiceTypeGetAllHandler(
            IMapper mapper,
            IInvoiceTypeQueryService invoiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeQueryService = invoiceTypeQueryService;
            _invoiceTypeQueryService.NotNull(nameof(invoiceTypeQueryService));
        }

        public async Task<ICollection<InvoiceTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var invoiceType = await _invoiceTypeQueryService.Get();
            if (invoiceType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<InvoiceTypeGetDto>>(invoiceType);
        }
    }
}
