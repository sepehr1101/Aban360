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
                .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6)
                .Must(IsDigit).WithMessage(ExceptionLiterals.MustDigit);

            RuleFor(f => f.SelectedServices)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(s => s.Count() > 0).WithMessage(ExceptionLiterals.InvalidZeroServiceSelected);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 100).WithMessage(ExceptionLiterals.Between5And100);

            RuleFor(f => f.PhoneNumber).
                Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.NotificationNumber)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

        }
    }
}
