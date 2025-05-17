using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.TotalApi.Validations
{
    public class TotalApiCreateValidator : BaseValidator<TotalApiCreateDto>
    {
        public TotalApiCreateValidator()
        {
            #region Estate
            RuleFor(e => e.Estate.ConstructionTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.EstateBoundTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.PostalCode)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(e => e.Estate.X)
                .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

            RuleFor(e => e.Estate.Y)
                .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

            RuleFor(e => e.Estate.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(e => e.Estate.MunipulityId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UsageSellId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UsageConsumtionId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UnitDomesticWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UnitCommercialWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UnitOtherWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UnitDomesticSewage)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UnitOtherSewage)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.EmptyUnit)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.HouseholdNumber)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.Premises)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ImprovementsOverall)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ImprovementsDomestic)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ImprovementsCommercial)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ImprovementsOther)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ContractualCapacity)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.Storeys)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.UserId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.ValidFrom)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(e => e.Estate.CapacityCalculationIndexId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
            #endregion

            #region WaterMeter
            RuleFor(f => f.WaterMeter.BillId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .MaximumLength(13).WithMessage(ExceptionLiterals.NotMoreThan13)
              .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6);

            RuleFor(f => f.WaterMeter.UseStateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.WaterMeter.SubscriptionTypeId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.WaterMeter.MeterDiameterId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);

            RuleFor(f => f.WaterMeter.MeterProducerId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterMaterialId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterUseTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(e => e.WaterMeter.UserId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNull);


            RuleFor(e => e.WaterMeter.EstateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);


            RuleFor(e => e.WaterMeter.WaterMeterInstallationMethodId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
            #endregion

            #region Siphon
            RuleForEach(s => s.siphons).ChildRules(siphon =>
            {
                siphon.RuleFor(s => s.SiphonDiameterId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

                siphon.RuleFor(s => s.SiphonTypeId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                    .NotNull().WithMessage(ExceptionLiterals.NotNull);

                siphon.RuleFor(s => s.SiphonMaterialId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                   .NotNull().WithMessage(ExceptionLiterals.NotNull);

                siphon.RuleFor(s => s.WaterMeterId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                   .NotNull().WithMessage(ExceptionLiterals.NotNull);

                siphon.RuleFor(s => s.UserId)
                  .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                  .NotNull().WithMessage(ExceptionLiterals.NotNull)
                  .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNull);

            });
            #endregion

            #region Individual
            RuleForEach(i => i.individuals).ChildRules(individual =>
            {
                individual.RuleFor(f => f.IndividualTypeId)
                              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                              .NotNull().WithMessage(ExceptionLiterals.NotNull);

                individual.RuleFor(f => f.WaterMeterId)
                              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                              .NotNull().WithMessage(ExceptionLiterals.NotNull);

                individual.RuleFor(f => f.FullName)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                   .NotNull().WithMessage(ExceptionLiterals.NotNull)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                individual.RuleFor(s => s.UserId)
                  .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                  .NotNull().WithMessage(ExceptionLiterals.NotNull)
                  .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNull);
            });
            #endregion
        }
    }
}
