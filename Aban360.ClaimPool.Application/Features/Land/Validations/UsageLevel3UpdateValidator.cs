using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class UsageLevel3UpdateValidator : BaseValidator<UsageLevel3UpdateDto>
    {
        public UsageLevel3UpdateValidator()
        {
            RuleFor(f => f.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.UsageLevel2Id)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}