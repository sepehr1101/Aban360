using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualEstateRelationTypeUpdateValidator : BaseValidator<IndividualEstateRelationTypeUpdateDto>
    {
        public IndividualEstateRelationTypeUpdateValidator()
        {

            RuleFor(f => f.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll);
        }
    }
}
