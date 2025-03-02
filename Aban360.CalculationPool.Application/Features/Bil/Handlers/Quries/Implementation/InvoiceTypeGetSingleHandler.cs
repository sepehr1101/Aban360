using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class InvoiceTypeGetSingleHandler : IInvoiceTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeQueryService _invoiceTypeQueryService;
        public InvoiceTypeGetSingleHandler(
            IMapper mapper,
            IInvoiceTypeQueryService invoiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeQueryService = invoiceTypeQueryService;
            _invoiceTypeQueryService.NotNull(nameof(invoiceTypeQueryService));
        }

        public async Task<InvoiceTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var invoiceType = await _invoiceTypeQueryService.Get(id);
            if (invoiceType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<InvoiceTypeGetDto>(invoiceType);
        }
    }
}
