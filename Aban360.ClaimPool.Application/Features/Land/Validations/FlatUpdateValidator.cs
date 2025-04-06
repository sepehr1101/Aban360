using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class FlatUpdateValidator:AbstractValidator<FlatUpdateDto>
    {
        public FlatUpdateValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");
            
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
