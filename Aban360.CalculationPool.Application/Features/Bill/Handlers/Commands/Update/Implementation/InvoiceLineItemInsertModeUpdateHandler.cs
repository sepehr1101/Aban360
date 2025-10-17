using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class InvoiceLineItemInsertModeUpdateHandler : IInvoiceLineItemInsertModeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        private readonly IValidator<InvoiceLineItemInsertModeUpdateDto> _validator;
        public InvoiceLineItemInsertModeUpdateHandler(
            IMapper mapper,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService,
            IValidator<InvoiceLineItemInsertModeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceLineItemInsertMode);
        }
    }
}
