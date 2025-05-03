using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestIndividualDiscountTypeUpdateValidator : BaseValidator<RequestIndividualDiscountTypeUpdateDto>
    {
        public RequestIndividualDiscountTypeUpdateValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(f => f.IndividualId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.DiscountTypeId)
                .IsInEnum().WithErrorCode(ExceptionLiterals.MustEnum)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.UserId)
                .IsInEnum().WithErrorCode(ExceptionLiterals.MustEnum)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.ExpireDate)
                .IsInEnum().WithErrorCode(ExceptionLiterals.MustEnum)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

        }
    }
}