using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Validations
{
    public class UseStateCreateValidator : BaseValidator<UseStateCreateDto>
    {
        public UseStateCreateValidator()
        {
            RuleFor(f => f.Id)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .IsInEnum().WithErrorCode(ExceptionLiterals.MustEnum);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}
