using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Validations
{
    public class SiphonMaterialCreateValidator : BaseValidator<SiphonMaterialCreateDto>
    {
        public SiphonMaterialCreateValidator()
        {
            RuleFor(s => s.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}
