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
    internal sealed class InvoiceLineItemInsertModeCreateHandler : IInvoiceLineItemInsertModeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeCommandService _invoiceLineItemInsertModeCommandService;
        private readonly IValidator<InvoiceLineItemInsertModeCreateDto> _validator;
        public InvoiceLineItemInsertModeCreateHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeCommandService invoiceLineItemInsertModeCommandService,
            IValidator<InvoiceLineItemInsertModeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeCommandService = invoiceLineItemInsertModeCommandService;
            _invoiceLineItemInsertModeCommandService.NotNull(nameof(invoiceLineItemInsertModeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            InvoiceLineItemInsertMode invoiceLineItemInsertMode = _mapper.Map<InvoiceLineItemInsertMode>(createDto);
            await _invoiceLineItemInsertModeCommandService.Add(invoiceLineItemInsertMode);
        }
    }
}
