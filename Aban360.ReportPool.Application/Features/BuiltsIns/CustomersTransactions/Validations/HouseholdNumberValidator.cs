using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class HouseholdNumberValidator : BaseValidator<HouseholdNumberInputDto>
    {
        public HouseholdNumberValidator()
        {
            RuleFor(customer => customer.FromHouseholdDateJalali)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToHouseholdDateJalali)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ZoneIds)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
               .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input. FromHouseholdDateJalali,
                                                                                               input.ToHouseholdDateJalali)).IsValid)
               .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromHouseholdDateJalali,
                                                                                               input.ToHouseholdDateJalali)).ErrorMessage);
        }
    }
}
