using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestIndividualDiscountTypeCreateValidator : BaseValidator<RequestIndividualDiscountTypeCreateDto>
    {
        public RequestIndividualDiscountTypeCreateValidator()
        {
            RuleFor(f => f.IndividualId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DiscountTypeId)
                .IsInEnum().WithErrorCode(ExceptionLiterals.MustEnum)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

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