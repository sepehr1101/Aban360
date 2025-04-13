using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestSubscriptionCreateValidator : BaseValidator<RequestSubscriptionCreateDto>
    {
        public RequestSubscriptionCreateValidator()
        {
            #region Estate
            RuleFor(e => e.Estate.ConstructionTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.EstateBoundTypeId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.PostalCode)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(e => e.Estate.X)
                .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

            RuleFor(e => e.Estate.Y)
                .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

            RuleFor(e => e.Estate.Address)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(e => e.Estate.MunipulityId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UsageSellId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UsageConsumtionId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UnitDomesticWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UnitCommercialWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UnitOtherWater)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UnitDomesticSewage)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.UnitOtherSewage)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.EmptyUnit)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.HouseholdNumber)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.Premises)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.ImprovementsOverall)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.ImprovementsDomestic)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.ImprovementsCommercial)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.ImprovementsOther)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.ContractualCapacity)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(e => e.Estate.Storeys)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            #endregion

            #region Flat
            RuleForEach(f => f.Flats).ChildRules(flat =>
            {
                flat.RuleFor(f => f.PostalCode)
                        .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                        .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                        .Length(10).WithMessage(ExceptionLiterals.Equal10);
            });
            #endregion

            #region Individual
            RuleForEach(i => i.Individuals).ChildRules(individual =>
            {
                individual.RuleFor(f => f.IndividualTypeId)
                              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                individual.RuleFor(f => f.FullName)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
            });
            #endregion

            #region Siphon
            RuleForEach(s => s.Siphons).ChildRules(siphon =>
            {
                siphon.RuleFor(s => s.SiphonDiameterId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                siphon.RuleFor(s => s.SiphonTypeId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                siphon.RuleFor(s => s.SiphonMaterialId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            });
            #endregion

            #region WaterMeter
            RuleFor(f => f.WaterMeter.BillId)
         .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
         .NotNull().WithMessage(ExceptionLiterals.NotNUll)
         .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);

            RuleFor(f => f.WaterMeter.UseStateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.WaterMeter.SubscriptionTypeId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.WaterMeter.MeterDiameterId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);

            RuleFor(f => f.WaterMeter.MeterProducerId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterMaterialId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.WaterMeter.MeterUseTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);
            #endregion
        }
    }
}
