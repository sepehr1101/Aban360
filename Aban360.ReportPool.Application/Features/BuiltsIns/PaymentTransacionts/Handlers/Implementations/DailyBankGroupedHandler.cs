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
    internal sealed class DailyBankGroupedHandler : IDailyBankGroupedHandler
    {
        private readonly IDailyBankGroupedQueryService _dailyBankGroupedQueryService;
        private readonly IValidator<DailyBankGroupedInputDto> _validator;
        public DailyBankGroupedHandler(
            IDailyBankGroupedQueryService dailyBankGroupedQueryService,
            IValidator<DailyBankGroupedInputDto> validator)
        {
            _dailyBankGroupedQueryService = dailyBankGroupedQueryService;
            _dailyBankGroupedQueryService.NotNull(nameof(dailyBankGroupedQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>> Handle(DailyBankGroupedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto> dailyBankGrouped = await _dailyBankGroupedQueryService.GetInfo(input);
            return dailyBankGrouped;
        }
    }
}
