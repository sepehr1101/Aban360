using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class AssessmentSetTimeValidator : BaseValidator<AssessmentSetTimeInputDto>
    {
        public AssessmentSetTimeValidator()
        {
            RuleFor(f => f.TrackNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.AssessmentCode)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.AssessmentDateJalali)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .Must(IsValidDateJalali).WithMessage(ExceptionLiterals.InvalidDate);
        }
    }
}
