using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class InvoiceStatusGetAllHandler : IInvoiceStatusGetAllHandler
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
            var invoiceStatus = await _invoiceStatusQueryService.Get();
            if (invoiceStatus == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<InvoiceStatusGetDto>>(invoiceStatus);
        }
    }
}
