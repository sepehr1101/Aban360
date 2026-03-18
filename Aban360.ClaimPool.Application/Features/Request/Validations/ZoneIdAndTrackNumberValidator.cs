using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class ZoneIdAndTrackNumberValidator : BaseValidator<ZoneIdAndTrackNumber>
    {
        public ZoneIdAndTrackNumberValidator()
        {
            RuleFor(f => f.TrackNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ZoneId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
