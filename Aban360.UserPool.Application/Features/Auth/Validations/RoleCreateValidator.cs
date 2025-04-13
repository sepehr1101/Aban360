using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class RoleCreateValidator : BaseValidator<RoleCreateDto>
    {
        public RoleCreateValidator()
        {
            RuleFor(u => u.Name)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.Title)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage("Not More than 255");
        }
    }
}
