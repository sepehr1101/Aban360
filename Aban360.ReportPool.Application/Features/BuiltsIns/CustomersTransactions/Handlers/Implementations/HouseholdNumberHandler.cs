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
    internal sealed class HouseholdNumberHandler : IHouseholdNumberHandler
    {
        private readonly IHouseholdNumberQueryService _householdNumberQueryService;
        private readonly IValidator<HouseholdNumberInputDto> _validator;
        public HouseholdNumberHandler(
            IHouseholdNumberQueryService householdNumberQueryService,
            IValidator<HouseholdNumberInputDto> validator)
        {
            _householdNumberQueryService = householdNumberQueryService;
            _householdNumberQueryService.NotNull(nameof(householdNumberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>> Handle(HouseholdNumberInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            
            input.ToHouseholdDateJalali=ReduceYear(input.ToHouseholdDateJalali);
            string lastDateJalai = ReduceYear(DateTime.Now.ToShortPersianDateString());
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto> householdNumber = await _householdNumberQueryService.GetInfo(input,lastDateJalai);
         
            return householdNumber;
        }
        private string ReduceYear(string jalaliDate)
        {                          
            DateOnly? gregorianDate = jalaliDate.ToGregorianDateOnly();
            if (!gregorianDate.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }
            return gregorianDate
                    .Value
                    .AddYears(-1)
                    .ToShortPersianDateString();
        }
    }
}
