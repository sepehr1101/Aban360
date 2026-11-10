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
               .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.FirstName))
               .Length(3, 15).WithMessage(string.Join(" , ", ExceptionLiterals.Between3And15, ExceptionLiterals.FirstName));

            RuleFor(f => f.Surname)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Surname))
                .Length(5, 25).WithMessage(string.Join(" , ", ExceptionLiterals.Between5And25, ExceptionLiterals.Surname));

            RuleFor(f => f.PhoneNumber)
                .Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.MobileNumber))
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.NationalCode)
                 .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.NationalCode))
                 .Must(IsValidNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.Address)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Address))
                .Length(5, 100).WithMessage(string.Join(" , ", ExceptionLiterals.Between5And100, ExceptionLiterals.Address));

            RuleFor(f => f.PostalCode)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.PostalCode))
                .Must(IsValidPostalCode).WithMessage(string.Join(" , ", ExceptionLiterals.PostalCodeFormat, ExceptionLiterals.PostalCode));

            RuleFor(f => f.ZoneId)
               .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Zone))
               .GreaterThan(0).WithMessage(string.Join(" , ", ExceptionLiterals.GreaterThanZero, ExceptionLiterals.Zone));

            RuleFor(f => f.SelectedServices)
               .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.InvalidZeroServiceSelected))
               .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.InvalidZeroServiceSelected))
                .Must(s => s.Count() > 0).WithMessage(ExceptionLiterals.InvalidZeroServiceSelected);
        }
    }
}
