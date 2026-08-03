using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class PendingPaymentsSummaryHandler : IPendingPaymentsSummaryHandler
    {
        private readonly IPendingPaymentsQueryService _pendingPaymentsQueryService;
        private readonly IValidator<PendingPaymentsSummaryDto> _validator;
        public PendingPaymentsSummaryHandler(
            IPendingPaymentsQueryService pendingPaymentsQueryService,
            IValidator<PendingPaymentsSummaryDto> validator)
        {
            _pendingPaymentsQueryService = pendingPaymentsQueryService;
            _pendingPaymentsQueryService.NotNull(nameof(pendingPaymentsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto>> Handle(PendingPaymentsSummaryDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentSummaryDataOutputDto> pendingPayments = await _pendingPaymentsQueryService.GetSummary(input);
            return pendingPayments;
        }
    }
}
