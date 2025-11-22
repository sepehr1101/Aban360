using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class TariffConstantUdpateValidator : BaseValidator<TariffConstantUpdateDto>
    {
        public TariffConstantUdpateValidator()
        {
            RuleFor(t => t.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.Value)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.Condition)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.Key)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.FromDateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(t => t.ToDateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(t => t.Description)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);
        }
    }
}
