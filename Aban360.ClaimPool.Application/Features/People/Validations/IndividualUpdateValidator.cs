using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualUpdateValidator : AbstractValidator<IndividualUpdateDto>
    {
        public IndividualUpdateValidator()
        {
            RuleFor(f => f.Id)
                          .NotEmpty().WithMessage("Not Empty")
                          .NotNull().WithMessage("Not Nyull");

            RuleFor(f => f.WaterMeterId)
                          .NotEmpty().WithMessage("Not Empty")
                          .NotNull().WithMessage("Not Nyull");

            RuleFor(f => f.FullName)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull")
               .MaximumLength(255).WithMessage("less than 255");

        }
    }
}
