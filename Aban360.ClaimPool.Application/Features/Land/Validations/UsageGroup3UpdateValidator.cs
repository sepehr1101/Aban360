using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class UsageGroup3UpdateValidator : BaseValidator<UsageGroup3UpdateDto>
    {
        public UsageGroup3UpdateValidator()
        {
            RuleFor(f => f.Id)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Group2Id)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.UsageId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
