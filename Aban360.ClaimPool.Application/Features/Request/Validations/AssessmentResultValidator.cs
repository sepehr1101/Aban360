using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class AssessmentResultValidator : BaseValidator<AssessmentResultInputDto>
    {
        public AssessmentResultValidator()
        {
            RuleFor(f => f.TrackingId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ReadingNumber)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(f => f.CounterType)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.TrackNumber)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ServiceGroupId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CustomerNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero); 

            RuleFor(f => f.NeighbourBillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.NotificationMobile)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.UsageId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.MeterDiameterId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.BranchTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.DiscountTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.ResultId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.PhoneNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.MobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.FirstName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.Between3And15);

            RuleFor(f => f.Surname)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 25).WithMessage(ExceptionLiterals.Between5And25);

            RuleFor(f => f.Premises)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.ImprovementOverall)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.ImprovementDomestic)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.ImprovementCommertial)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.Siphon100)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.Siphon125)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.Siphon150)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.Siphon200)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MainSiphon)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.CommonSiphon)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.ContractualCapacity)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.HouseValue)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.CommertialUnit)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.DomesticUnit)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.OtherUnit)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.DiscountCount)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.NationalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.FatherName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.InvlaidStringLength);

            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidPostalCode).WithMessage(ExceptionLiterals.PostalCodeFormat);

            RuleFor(f => f.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 100).WithMessage(ExceptionLiterals.Between5And100); 

            RuleFor(f => f.Description)
                .MaximumLength(60).WithMessage(ExceptionLiterals.NotMoreThan100);

        }
    }
}
