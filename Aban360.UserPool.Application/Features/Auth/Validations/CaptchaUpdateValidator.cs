using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;
using Aban360.Common.Literals;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class CaptchaUpdateValidator : BaseValidator<CaptchaUpdateDto>
    {
        public CaptchaUpdateValidator()
        {
            RuleFor(u => u.CaptchaLanguageId)
                    .NotNull().WithMessage(ExceptionLiterals.NotNull)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.CaptchaDisplayModeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.ShowThousandSeperator)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.FontName)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.FontSize)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.BackColor)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.ExpiresAfter)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.RateLimit)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.Noise)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.EncryptionKey)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(1023).WithMessage("Not More than 1023");

            RuleFor(u => u.NonceKey)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(1023).WithMessage("Not More than 1023");

            RuleFor(u => u.Direction)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(3).WithMessage("Not More than 3");

            RuleFor(u => u.Min)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.Max)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(u => u.Title)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(255).WithMessage("Not More than 255");

            RuleFor(u => u.IsSelected)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
