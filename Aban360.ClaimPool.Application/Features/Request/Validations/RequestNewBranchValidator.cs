using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class RequestNewBranchValidator : BaseValidator<RequestNewBranchInputDto>
    {
        public RequestNewBranchValidator()
        {
            RuleFor(f => f.FirstName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.Between3And15);

            RuleFor(f => f.Surname)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 25).WithMessage(ExceptionLiterals.Between5And25);

            RuleFor(f => f.FatherName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.InvlaidStringLength);

            RuleFor(f => f.PhoneNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.NationalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.CertificataNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .MinimumLength(3).WithMessage(ExceptionLiterals.NotLessThan3)
                .Must(IsDigit).WithMessage(ExceptionLiterals.MustDigit);

            RuleFor(f => f.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 100).WithMessage(ExceptionLiterals.Between5And100);

            RuleFor(f => f.NeighbourBillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6)
                .Must(IsDigit).WithMessage(ExceptionLiterals.MustDigit);

            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidPostalCode).WithMessage(ExceptionLiterals.PostalCodeFormat);
        }
    }
}
