using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class TariffCalculationModeCreateValidator : BaseValidator<TariffCalculationModeCreateDto>
    {
        public TariffCalculationModeCreateValidator()
        {
            RuleFor(t => t.Id)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.Description)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

        }
    }
}
