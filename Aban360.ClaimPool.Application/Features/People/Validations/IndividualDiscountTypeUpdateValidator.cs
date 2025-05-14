using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Validations
{
    public class IndividualDiscountTypeUpdateValidator : BaseValidator<IndividualDiscountTypeUpdateDto>
    {
        public IndividualDiscountTypeUpdateValidator()
        {
            RuleFor(f => f.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.IndividualId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DiscountTypeId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.UserId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ExpireDate)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
