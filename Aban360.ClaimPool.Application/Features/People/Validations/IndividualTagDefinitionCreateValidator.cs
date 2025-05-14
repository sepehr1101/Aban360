using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualTagDefinitionCreateValidator : BaseValidator<IndividualTagDefinitionCreateDto>
    {
        public IndividualTagDefinitionCreateValidator()
        {
            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.Color)
               .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);
        }
    }
}
