using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Validations
{
    public class TrackingDetailValidator : BaseValidator<TrackingDetailGetDto>
    {
        public TrackingDetailValidator()
        {
            RuleFor(t => t.TrackId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.ZoneId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
