using Aban360.CalculationPool.Application.Features.Bill.Validations;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Validations
{
    public class TariffCalculationImaginaryValidator : BaseValidator<TariffTestImaginaryInput>
    {
        public TariffCalculationImaginaryValidator()
        {
            RuleFor(i => i.CustomerNumber)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.CurrentWaterMeterDate)
                .LessThan(i => i.PreviousWaterMeterDate)
                .WithMessage(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.PreviousWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.InstallationDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ContractualCapacity)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.HouseholdNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitDomesticWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitCommercialWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitOtherWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitDomesticWater)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitCommercialSewage)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UnitOtherSewage)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.EmptyUnit)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ConstructionTypeId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UsageConsumptionId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.UsageSellId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.HeadquarterId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ProvinceId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.RegionId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ZoneId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.MunicipalityId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.PreviousWaterMeterNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.CurrentWaterMeterNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.PreviousWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.CurrentWaterMeterDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);


        }
    }
}
