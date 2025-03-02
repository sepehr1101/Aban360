using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    public class InvoiceStatusCreateHandler : IInvoiceStatusCreateHandler
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
            var invoiceStatus = _mapper.Map<InvoiceStatus>(createDto);
            if (invoiceStatus == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceStatusCommandService.Add(invoiceStatus);
        }
    }
}
