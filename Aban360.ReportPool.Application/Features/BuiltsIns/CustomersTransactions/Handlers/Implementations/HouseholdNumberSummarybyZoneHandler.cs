using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class HouseholdNumberSummarybyZoneHandler : IHouseholdNumberSummarybyZoneHandler
    {
        private readonly IHouseholdNumberSummaryByZoneQueryService _householdNumberSummarybyZoneQueryService;
        private readonly IValidator<HouseholdNumberInputDto> _validator;
        public HouseholdNumberSummarybyZoneHandler(
            IHouseholdNumberSummaryByZoneQueryService householdNumberSummarybyZoneQueryService,
            IValidator<HouseholdNumberInputDto> validator)
        {
            _householdNumberSummarybyZoneQueryService = householdNumberSummarybyZoneQueryService;
            _householdNumberSummarybyZoneQueryService.NotNull(nameof(householdNumberSummarybyZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto>> Handle(HouseholdNumberInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            input.ToHouseholdDateJalali = ReduceYear(input.ToHouseholdDateJalali);
            string lastDateJalai = ReduceYear(DateTime.Now.ToShortPersianDateString());
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto> HouseholdNumberSummarybyZone = await _householdNumberSummarybyZoneQueryService.GetInfo(input, lastDateJalai);

            return HouseholdNumberSummarybyZone;
        }
        private string ReduceYear(string jalaliDate)
        {
            DateOnly? lastDateJalali = jalaliDate.ToGregorianDateOnly();
            if (!lastDateJalali.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }
            return lastDateJalali
                              .Value
                              .AddYears(-1)
                              .ToShortPersianDateString();
        }
    }
}
