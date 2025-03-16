using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceStatusCreateHandler : IInvoiceStatusCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusCommandService _invoiceStatusCommandService;
        public InvoiceStatusCreateHandler(
            IMapper mapper,
            IInvoiceStatusCommandService invoiceStatusCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusCommandService = invoiceStatusCommandService;
            _invoiceStatusCommandService.NotNull(nameof(invoiceStatusCommandService));
        }

        public async Task Handle(InvoiceStatusCreateDto createDto, CancellationToken cancellationToken)
        {
            InvoiceStatus invoiceStatus = _mapper.Map<InvoiceStatus>(createDto);
            await _invoiceStatusCommandService.Add(invoiceStatus);
        }
    }
}
