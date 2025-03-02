using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    public class InvoiceTypeCreateHandler : IInvoiceTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeCommandService _invoiceTypeCommandService;
        public InvoiceTypeCreateHandler(
            IMapper mapper,
            IInvoiceTypeCommandService invoiceTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeCommandService = invoiceTypeCommandService;
            _invoiceTypeCommandService.NotNull(nameof(invoiceTypeCommandService));
        }

        public async Task Handle(InvoiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var invoiceType = _mapper.Map<InvoiceType>(createDto);
            if (invoiceType == null)
            {
                throw new InvalidDataException();
            }
            await _invoiceTypeCommandService.Add(invoiceType);
        }
    }
}
