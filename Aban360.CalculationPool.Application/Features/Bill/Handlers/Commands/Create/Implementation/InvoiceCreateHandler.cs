using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceCreateHandler : IInvoiceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceCommandService _invoiceCommandService;
        private readonly IValidator<InvoiceCreateDto> _validator;
        public InvoiceCreateHandler(
            IMapper mapper,
            IInvoiceCommandService invoiceCommandService,
            IValidator<InvoiceCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceCommandService = invoiceCommandService;
            _invoiceCommandService.NotNull(nameof(invoiceCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(InvoiceCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Invoice invoice = _mapper.Map<Invoice>(createDto);
            await _invoiceCommandService.Add(invoice);
        }
    }
}
