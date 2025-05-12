using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class TariffCreateValidator : BaseValidator<TariffCreateDto>
    {
        public TariffCreateValidator()
        {
            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.LineItemTypeId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.OfferingId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.Condition)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.Formula)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.FromDateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(t => t.ToDateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(t => t.Description)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);
        }
    }
}
