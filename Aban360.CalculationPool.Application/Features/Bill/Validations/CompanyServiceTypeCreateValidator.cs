using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class CompanyServiceTypeCreateValidator : BaseValidator<CompanyServiceTypeCreateDto>
    {
        public CompanyServiceTypeCreateValidator()
        {
            RuleFor(o => o.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(o => o.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(o => o.Description)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(o => o.TariffCalculationModeId)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
        }
    }
}
