using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestIndividualTagCreateValidator : BaseValidator<IndividualTagRequestCreateDto>
    {
        public RequestIndividualTagCreateValidator()
        {
            RuleFor(f => f.IndividualId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.IndividualTagDefinitionId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

        }
    }
}