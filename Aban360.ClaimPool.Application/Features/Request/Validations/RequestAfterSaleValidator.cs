using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class RequestAfterSaleValidator : BaseValidator<RequestAfterSaleInputDto>
    {
        public RequestAfterSaleValidator()
        {
            RuleFor(f => f.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6);

            RuleFor(f => f.SelectedServices)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6);

            RuleFor(f => f.PhoneNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.NotificationNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
