using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualCreateValidator : AbstractValidator<IndividualCreateDto>
    {
        public IndividualCreateValidator()
        {
            RuleFor(f => f.WaterMeterId)
                          .NotEmpty().WithMessage("Not Empty")
                          .NotNull().WithMessage("Not Nyull");
            
            RuleFor(f => f.IndividualTypeId)
                          .NotEmpty().WithMessage("Not Empty")
                          .NotNull().WithMessage("Not Nyull");

            RuleFor(f => f.FullName)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull")
               .MaximumLength(255).WithMessage("less than 255");

        }
    }
}
