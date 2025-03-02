using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Implementation
{
    public class InvoiceStatusUpdateHandler : IInvoiceStatusUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusQueryService _invoiceStatusQueryService;
        public InvoiceStatusUpdateHandler(
            IMapper mapper,
            IInvoiceStatusQueryService invoiceStatusQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusQueryService = invoiceStatusQueryService;
            _invoiceStatusQueryService.NotNull(nameof(invoiceStatusQueryService));
        }

        public async Task Handle(InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var invoiceStatus = await _invoiceStatusQueryService.Get(updateDto.Id);
            if (invoiceStatus == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, invoiceStatus);
        }
    }
}
