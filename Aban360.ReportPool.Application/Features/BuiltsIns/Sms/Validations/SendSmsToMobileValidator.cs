using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Validations
{
     public class SendSmsToMobileValidator : BaseValidator<SendSmsToMobileInputDto>
    {
        public SendSmsToMobileValidator()
        {
            RuleFor(installation => installation.FromDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(installation => installation.ToDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(installation => installation.Mobile)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
