using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class TariffCalculationImaginaryValidator : BaseValidator<IntervalBillSubscriptionInfoImaginary>
    {
        public TariffCalculationImaginaryValidator()
        {
            RuleFor(i => i.CustomerNumber)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i => i.CurrentWaterMeterDate)
                .LessThan(i => i.PreviousWaterMeterDate)
                .WithMessage(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(i => i.PreviousWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.InstallationDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.ContractualCapacity)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.HouseholdNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitDomesticWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitCommercialWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitOtherWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitDomesticWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitCommercialSewage)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UnitOtherSewage)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.EmptyUnit)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.ConstructionTypeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UsageConsumptionId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.UsageSellId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.HeadquarterId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.ProvinceId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.RegionId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.ZoneId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.MunicipalityId)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.PreviousWaterMeterNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.CurrentWaterMeterNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.PreviousWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i=>i.CurrentWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);


        }
    }
}
