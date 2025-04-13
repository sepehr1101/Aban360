using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class UserUpdateValidator:BaseValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(u => u.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.FullName)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.DisplayName)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.Mobile)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .Length(11).WithMessage(ExceptionLiterals.NotMoreThan11);

            RuleFor(u => u.SelectedRoleIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.SelectedZoneIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.SelectedEndpointIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
        }
    }
}
