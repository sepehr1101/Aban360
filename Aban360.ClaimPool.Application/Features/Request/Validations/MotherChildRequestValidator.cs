using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class MotherChildRequestValidator : BaseValidator<MotherChildRequestInputDto>
    {
        public MotherChildRequestValidator()
        {
            RuleFor(f => f.TrackNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MotherCustomerNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Premises)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementOverall)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementDomestic)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementCommercial)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CommercialUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DomesticUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.OtherUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
