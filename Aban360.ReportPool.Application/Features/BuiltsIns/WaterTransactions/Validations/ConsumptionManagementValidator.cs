using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class ConsumptionManagementValidator : BaseValidator<ConsumptionManagementInputDto>
    {
        public ConsumptionManagementValidator()
        {
            RuleFor(c => c.FromBaseDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ToBaseDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.FromComparisonDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ToComparisonDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.FromMultiplier)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ToMultiplier)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ZoneIds)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
              .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromBaseDateJalali,
                                                                                              input.ToBaseDateJalali)).IsValid)
              .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromBaseDateJalali,
                                                                                                     input.ToBaseDateJalali)).ErrorMessage);

            RuleFor(input => input)
              .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromComparisonDateJalali,
                                                                                              input.ToComparisonDateJalali)).IsValid)
              .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromComparisonDateJalali,
                                                                                                     input.ToComparisonDateJalali)).ErrorMessage);
            
            RuleFor(input => input)
              .Must(input => ValidationBaseAndComperisonDate(input).IsValid)
              .WithMessage(input => ValidationBaseAndComperisonDate(input).ErrorMessage);

           
        }
        public static (bool IsValid, string ErrorMessage) ValidationBaseAndComperisonDate(ConsumptionManagementInputDto input)
        {
            if (input.FromComparisonDateJalali.CompareTo(input.FromBaseDateJalali) > 0 ||
                input.ToComparisonDateJalali.CompareTo(input.ToBaseDateJalali) > 0)
            {
                return (false, ExceptionLiterals.BaseBeforComparisonDate);
            }

            return (true, "");
        }
    }
}
