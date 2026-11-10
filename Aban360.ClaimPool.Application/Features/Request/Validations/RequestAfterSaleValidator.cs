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
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.BillId))
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.BillId))
                .MinimumLength(6).WithMessage(string.Join(" , ", ExceptionLiterals.NotLessThan6, ExceptionLiterals.BillId))
                .Must(IsDigit).WithMessage(string.Join(" , ", ExceptionLiterals.MustDigit, ExceptionLiterals.BillId));

            RuleFor(f => f.SelectedServices)
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.InvalidZeroServiceSelected))
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.InvalidZeroServiceSelected))
                .Must(s => s.Count() > 0).WithMessage(ExceptionLiterals.InvalidZeroServiceSelected);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.MobileNumber))
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.Address)
               .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Address))
               .Length(5, 100).WithMessage(string.Join(" , ", ExceptionLiterals.Between5And100, ExceptionLiterals.Address));

            RuleFor(f => f.PhoneNumber).
                Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.NotificationNumber)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

        }
    }
}
