using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceStatusCreateHandler : IInvoiceStatusCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusCommandService _invoiceStatusCommandService;
        private readonly IValidator<InvoiceStatusCreateDto> _validator;
        public InvoiceStatusCreateHandler(
            IMapper mapper,
            IInvoiceStatusCommandService invoiceStatusCommandService,
            IValidator<InvoiceStatusCreateDto> validator
)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusCommandService = invoiceStatusCommandService;
            _invoiceStatusCommandService.NotNull(nameof(invoiceStatusCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(InvoiceStatusCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InvoiceStatus invoiceStatus = _mapper.Map<InvoiceStatus>(createDto);
            await _invoiceStatusCommandService.Add(invoiceStatus);
        }
    }
}
