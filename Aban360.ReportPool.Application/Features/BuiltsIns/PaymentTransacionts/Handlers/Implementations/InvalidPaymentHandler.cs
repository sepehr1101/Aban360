using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class InvalidPaymentHandler : IInvalidPaymentHandler
    {
        private readonly IInvalidPaymentQueryService _invalidPaymentQueryService;
        private readonly IValidator<InvalidPaymentInputDto> _validator;
        public InvalidPaymentHandler(
            IInvalidPaymentQueryService invalidPaymentQueryService,
            IValidator<InvalidPaymentInputDto> validator)
        {
            _invalidPaymentQueryService = invalidPaymentQueryService;
            _invalidPaymentQueryService.NotNull(nameof(invalidPaymentQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>> Handle(InvalidPaymentInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto> invalidPayment = await _invalidPaymentQueryService.GetInfo(input);
            return invalidPayment;
        }
    }
}
