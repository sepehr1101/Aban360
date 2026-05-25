using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class PreAssessmentResultValidator : BaseValidator<PreAssessmentResultInputDto>
    {
        public PreAssessmentResultValidator()
        {
            RuleFor(f => f.TrackId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ResultId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
