using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Validations
{
    public class WaterMeterTagDefinitionUpdateValidator : BaseValidator<WaterMeterTagDefinitionUpdateDto>
    {
        public WaterMeterTagDefinitionUpdateValidator()
        {
            RuleFor(s => s.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(s => s.Color)
               .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);
        }
    }
}
