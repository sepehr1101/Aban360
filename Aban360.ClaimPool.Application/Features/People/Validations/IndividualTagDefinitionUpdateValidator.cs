using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualTagDefinitionUpdateValidator : BaseValidator<IndividualTagDefinitionUpdateDto>
    {
        public IndividualTagDefinitionUpdateValidator()
        {

            RuleFor(f => f.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.Color)
               .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);
        }
    }
}
