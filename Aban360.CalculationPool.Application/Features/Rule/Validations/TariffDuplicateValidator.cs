using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class TariffDuplicateValidator : BaseValidator<TariffDuplicateDto>
    {
        public TariffDuplicateValidator()
        {
            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);


            RuleFor(t => t.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

        }
    }
}
