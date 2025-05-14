using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class UserCreateValidator : BaseValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(u => u.FullName)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.DisplayName)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.Username)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage("Not More than 255");//Unique

            RuleFor(u => u.Password)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.Mobile)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .Length(11).WithMessage("Should 11 Char");

            RuleFor(u => u.SelectedRoleIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.SelectedZoneIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.SelectedEndpointIds)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }    
}
