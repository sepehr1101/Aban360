using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class MoshtrakUpdateValidator : BaseValidator<MoshtrakUpdateInputDto>
    {
        public MoshtrakUpdateValidator()
        {
            RuleFor(f => f.TrackNumber)
               .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.TrackNumber))
               .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.TrackNumber));

            RuleFor(f => f.CustomerNumber)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.CustomerNumber));

            RuleFor(f => f.ZoneId)
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Zone))
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.NotificationMobile)
                .Must(IsValidMobileNumberOrNull).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.UsageId)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Usage));

            RuleFor(f => f.MeterDiameterId)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.MeterDiameter))
                .GreaterThan(0).WithMessage(ExceptionLiterals.GreaterThanZero);

            RuleFor(f => f.BranchTypeId)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.BranchType));

            RuleFor(f => f.DiscountTypeId)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.DiscountType));

            RuleFor(f => f.PhoneNumber)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.PhoneNumber))
                .Must(IsValidPhoneNumber).WithMessage(ExceptionLiterals.PhoneNumberFormat);

            RuleFor(f => f.MobileNumber)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.MobileNumber))
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.FirstName)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.FirstName))
                .Length(3, 15).WithMessage(ExceptionLiterals.Between3And15);

            RuleFor(f => f.Surname)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Surname))
                .Length(5, 25).WithMessage(ExceptionLiterals.Between5And25);

            RuleFor(f => f.Premises)
             .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Premises))
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Premises));

            RuleFor(f => f.ImprovementOverall)
             .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.OverallImprovement))
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.OverallImprovement));

            RuleFor(f => f.ImprovementDomestic)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.DomesticImprovement));

            RuleFor(f => f.ImprovementCommercial)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.CommercialImprovement));

            RuleFor(f => f.Siphon100)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Siphon100));

            RuleFor(f => f.Siphon125)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Siphon125));

            RuleFor(f => f.Siphon150)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Siphon150));

            RuleFor(f => f.Siphon200)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Siphon200));

            RuleFor(f => f.MainSiphon)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.MainSiphon));

            RuleFor(f => f.CommonSiphon)
             .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.CommonSiphon));

            RuleFor(f => f.ContractualCapacity)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.ContractualCapacity));

            RuleFor(f => f.HouseValue)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.HouseValue));

            RuleFor(f => f.CommercialUnit)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.CommercialUnit));

            RuleFor(f => f.DomesticUnit)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.DomesticUnit));

            RuleFor(f => f.OtherUnit)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.OtherUnit));

            RuleFor(f => f.DiscountCount)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.DiscountType));

            RuleFor(f => f.NationalCode)
                .NotEmpty().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.NationalCode))
                .Must(IsValidNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.FatherName)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.FatherName))
                .Length(3, 15).WithMessage(ExceptionLiterals.InvlaidStringLength);

            RuleFor(f => f.PostalCode)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.PostalCode))
                .Must(IsValidPostalCode).WithMessage(ExceptionLiterals.PostalCodeFormat);

            RuleFor(f => f.Address)
                .NotNull().WithMessage(string.Join(" , ", ExceptionLiterals.NotNull, ExceptionLiterals.Address))
                .Length(5, 100).WithMessage(ExceptionLiterals.Between5And100);

            RuleFor(f => f.Description)
                .MaximumLength(60).WithMessage(ExceptionLiterals.NotMoreThan100);

        }
    }
}
