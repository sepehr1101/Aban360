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
    internal sealed class DebtorByDaySummaryHandler : IDebtorByDaySummaryHandler
    {
        private readonly IDebtorByDaySummaryQueryService _debtorByDayQueryService;
        private readonly IValidator<DebtorByDayInputDto> _validator;
        public DebtorByDaySummaryHandler(
            IDebtorByDaySummaryQueryService debtorByDayQueryService,
            IValidator<DebtorByDayInputDto> validator)
        {
            _debtorByDayQueryService = debtorByDayQueryService;
            _debtorByDayQueryService.NotNull(nameof(debtorByDayQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto>> Handle(DebtorByDayInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDaySummaryDataOutputDto> debtorByDay = await _debtorByDayQueryService.GetInfo(input);
            return debtorByDay;
        }
    }
}
