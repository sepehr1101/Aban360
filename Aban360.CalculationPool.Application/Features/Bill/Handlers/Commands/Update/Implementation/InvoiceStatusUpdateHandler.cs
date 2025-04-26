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
    internal sealed class InvoiceStatusUpdateHandler : IInvoiceStatusUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceStatusQueryService _invoiceStatusQueryService;
        private readonly IValidator<InvoiceStatusUpdateDto> _validator;
        public InvoiceStatusUpdateHandler(
            IMapper mapper,
            IInvoiceStatusQueryService invoiceStatusQueryService,
            IValidator<InvoiceStatusUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceStatusQueryService = invoiceStatusQueryService;
            _invoiceStatusQueryService.NotNull(nameof(invoiceStatusQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            InvoiceStatus invoiceStatus = await _invoiceStatusQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceStatus);
        }
    }
}
