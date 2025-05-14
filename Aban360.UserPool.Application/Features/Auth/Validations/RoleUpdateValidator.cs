using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class RoleUpdateValidator : BaseValidator<RoleUpdateDto>
    {
        public RoleUpdateValidator()
        {
            RuleFor(u => u.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.Name)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.Title)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.SensitiveInfo)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
