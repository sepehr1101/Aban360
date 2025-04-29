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
    internal sealed class InvoiceTypeCreateHandler : IInvoiceTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeCommandService _invoiceTypeCommandService;
        private readonly IValidator<InvoiceTypeCreateDto> _validator;
        public InvoiceTypeCreateHandler(
            IMapper mapper,
            IInvoiceTypeCommandService invoiceTypeCommandService,
            IValidator<InvoiceTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeCommandService = invoiceTypeCommandService;
            _invoiceTypeCommandService.NotNull(nameof(invoiceTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(InvoiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            InvoiceType invoiceType = _mapper.Map<InvoiceType>(createDto);
            await _invoiceTypeCommandService.Add(invoiceType);
        }
    }
}
