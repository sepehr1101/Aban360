using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class InvoiceStatusUpdateHandler : IInvoiceStatusUpdateHandler
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
            InvoiceStatus invoiceStatus = await _invoiceStatusQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceStatus);
        }
    }
}
