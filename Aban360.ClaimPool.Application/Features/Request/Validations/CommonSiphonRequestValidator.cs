using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class CommonSiphonRequestValidator : BaseValidator<CommonSiphonRequestInputDto>
    {
        public CommonSiphonRequestValidator()
        {
            RuleFor(f => f.TrackNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MotherCustomerNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon100)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon125)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon150)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon200)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
