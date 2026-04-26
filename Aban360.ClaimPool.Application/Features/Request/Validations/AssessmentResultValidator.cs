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
             .NotNull().WithMessage(ExceptionLiterals.NotNull);//
            
            RuleFor(f => f.CounterType)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.TrackNumber)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ServiceGroupId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CustomerNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.NeighbourBillId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ZoneId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.NotificationMobile)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.UsageId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MeterDiameterId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull); 

            RuleFor(f => f.BranchTypeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DiscountTypeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ResultId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.PhoneNumber)
                .Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.MobileNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.FirstName)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.Between3And15);

            RuleFor(f => f.Surname)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 25).WithMessage(ExceptionLiterals.Between5And25);

            RuleFor(f => f.Premises)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementOverall)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementDomestic)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ImprovementCommertial)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon100)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon125)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon150)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Siphon200)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MainSiphon)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CommonSiphon)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ContractualCapacity)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.HouseValue)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CommertialUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DomesticUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.OtherUnit)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.DiscountCount)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.NationalCode)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.FatherName)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Length(3, 15).WithMessage(ExceptionLiterals.InvlaidStringLength);

            RuleFor(f => f.PostalCode)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidPostalCode).WithMessage(ExceptionLiterals.PostalCodeFormat);

            RuleFor(f => f.Address)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Length(5, 100).WithMessage(ExceptionLiterals.Between5And100); 

            RuleFor(f => f.Description)
                .MaximumLength(60).WithMessage(ExceptionLiterals.NotMoreThan100);

        }
    }
}
