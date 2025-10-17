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
    internal sealed class InvoiceTypeUpdateHandler : IInvoiceTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeQueryService _invoiceTypeQueryService;
        private readonly IValidator<InvoiceTypeUpdateDto> _validator;

        public InvoiceTypeUpdateHandler(
            IMapper mapper,
            IInvoiceTypeQueryService invoiceTypeQueryService,
            IValidator<InvoiceTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeQueryService = invoiceTypeQueryService;
            _invoiceTypeQueryService.NotNull(nameof(invoiceTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            InvoiceType invoiceType = await _invoiceTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceType);
        }
    }
}
