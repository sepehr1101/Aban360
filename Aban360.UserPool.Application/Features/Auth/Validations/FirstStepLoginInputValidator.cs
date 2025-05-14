using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class FirstStepLoginInputValidator : BaseValidator<FirstStepLoginInput>
    {
        public FirstStepLoginInputValidator()
        {
            RuleFor(u => u.Username)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.Password)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.CaptchaToken)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);//todo: length

            RuleFor(u => u.CaptchaInputText)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);//todo: length

        }
    }
}
