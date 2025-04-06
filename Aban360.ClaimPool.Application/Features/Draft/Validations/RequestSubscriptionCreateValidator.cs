using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestSubscriptionCreateValidator : AbstractValidator<RequestSubscriptionCreateDto>
    {
        public RequestSubscriptionCreateValidator()
        {
            //Estate
            RuleFor(e => e.Estate.ConstructionTypeId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.EstateBoundTypeId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.PostalCode)
                .Length(10).WithMessage("____PostalCode Must 10 Chat");

            RuleFor(e => e.Estate.X)
                .MaximumLength(31).WithMessage("X less than 31");

            RuleFor(e => e.Estate.Y)
                .MaximumLength(31).WithMessage("Y less than 31");

            RuleFor(e => e.Estate.Address)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull")
                .MaximumLength(1023).WithMessage("Address less than 1023");

            RuleFor(e => e.Estate.MunipulityId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UsageSellId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UsageConsumtionId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UnitDomesticWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UnitCommercialWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UnitOtherWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UnitDomesticSewage)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.UnitOtherSewage)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.EmptyUnit)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.HouseholdNumber)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.Premises)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.ImprovementsOverall)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.ImprovementsDomestic)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.ImprovementsCommercial)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.ImprovementsOther)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.ContractualCapacity)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Estate.Storeys)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");


            //Flat
            RuleForEach(f => f.Flats).ChildRules(flat =>
            {
                flat.RuleFor(f => f.PostalCode)
                        .NotEmpty().WithMessage("Not Empty")
                        .NotNull().WithMessage("Not Nyull")
                        .Length(10).WithMessage("must 10 char");
            });


            //Individual
            RuleForEach(i => i.Individuals).ChildRules(individual =>
            {
                individual.RuleFor(f => f.IndividualTypeId)
                              .NotEmpty().WithMessage("Not Empty")
                              .NotNull().WithMessage("Not Nyull");

                individual.RuleFor(f => f.FullName)
                   .NotEmpty().WithMessage("Not Empty")
                   .NotNull().WithMessage("Not Nyull")
                   .MaximumLength(255).WithMessage("less than 255");

                individual.RuleFor(f => f.NationalId)
                  .Length(10).WithMessage("less than 255");
            });


            //Siphon
            RuleForEach(s => s.Siphons).ChildRules(siphon =>
            {
                siphon.RuleFor(s => s.SiphonDiameterId)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull");

                siphon.RuleFor(s => s.SiphonTypeId)
                    .NotEmpty().WithMessage("Not Empty")
                    .NotNull().WithMessage("Not Nyull");

                siphon.RuleFor(s => s.SiphonMaterialId)
                   .NotEmpty().WithMessage("Not Empty")
                   .NotNull().WithMessage("Not Nyull");
            });


            //WaterMeter
            RuleFor(f => f.WaterMeter.BillId)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull")
               .MaximumLength(15).WithMessage("less than 15");

            RuleFor(f => f.WaterMeter.UseStateId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(f => f.WaterMeter.SubscriptionTypeId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");
        }
    }
}
