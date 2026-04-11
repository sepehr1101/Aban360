using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Requests.Validations
{
    public class TrackingStepGroupValidator : BaseValidator<TrackingInputDto>
    {
        public TrackingStepGroupValidator()
        {
            RuleFor(customer => customer.FromDateJalali)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToDateJalali)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ZoneIds)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
