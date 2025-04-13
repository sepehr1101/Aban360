using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class FlatUpdateValidator:BaseValidator<FlatUpdateDto>
    {
        public FlatUpdateValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(f => f.EstateId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);
        }
    }
}
