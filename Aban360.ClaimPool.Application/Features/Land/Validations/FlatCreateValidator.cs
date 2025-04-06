using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class FlatCreateValidator:AbstractValidator<FlatCreateDto>
    {
        public FlatCreateValidator()
        {
            RuleFor(f => f.EstateId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");
            
            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull")
                .Length(10).WithMessage("must 10 char");
        }
    }
}
