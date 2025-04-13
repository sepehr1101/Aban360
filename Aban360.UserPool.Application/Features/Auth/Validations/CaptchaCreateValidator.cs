using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using FluentValidation;
using Aban360.Common.Literals;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public class CaptchaCreateValidator : BaseValidator<CaptchaCreateDto>
    {
        public CaptchaCreateValidator()
        {
            RuleFor(u => u.CaptchaLanguageId)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.CaptchaDisplayModeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.ShowThousandSeperator)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.FontName)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.FontSize)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.BackColor)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.ExpiresAfter)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.RateLimit)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.Noise)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.EncryptionKey)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(u => u.NonceKey)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(u => u.Direction)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(3).WithMessage(ExceptionLiterals.NotMoreThan3);

            RuleFor(u => u.Min)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.Max)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(u => u.Title)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(u => u.IsSelected)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
        }
    }
}
