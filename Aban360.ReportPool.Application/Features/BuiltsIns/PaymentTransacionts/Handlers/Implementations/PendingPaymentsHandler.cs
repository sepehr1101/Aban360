using Aban360.Common.Excel;
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
    internal sealed class PendingPaymentsHandler : IPendingPaymentsHandler
    {
        private readonly IPendingPaymentsQueryService _pendingPaymentsQueryService;
        private readonly IValidator<PendingPaymentsInputDto> _validator;
        public PendingPaymentsHandler(
            IPendingPaymentsQueryService pendingPaymentsQueryService,
            IValidator<PendingPaymentsInputDto> validator)
        {
            _pendingPaymentsQueryService = pendingPaymentsQueryService;
            _pendingPaymentsQueryService.NotNull(nameof(pendingPaymentsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>> Handle(PendingPaymentsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments = await _pendingPaymentsQueryService.GetInfo(input);
            return pendingPayments;
        }
    }
}
