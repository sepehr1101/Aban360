using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class CustomerMobileUpdateValidator : BaseValidator<CustomerMobileUpdateInputDto>
    {
        public CustomerMobileUpdateValidator()
        {
            RuleFor(f => f.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Length(11).WithMessage(ExceptionLiterals.Equal11);
        }
    }
}