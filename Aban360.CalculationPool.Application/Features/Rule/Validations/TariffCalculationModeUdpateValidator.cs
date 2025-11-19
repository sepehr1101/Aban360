using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class TariffCalculationModeUdpateValidator : BaseValidator<TariffCalculationModeUpdateDto>
    {
        public TariffCalculationModeUdpateValidator()
        {
            RuleFor(t => t.Id)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.Description)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

        }
    }
}
