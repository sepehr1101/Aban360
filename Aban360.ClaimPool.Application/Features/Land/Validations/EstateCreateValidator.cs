using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class EstateCreateValidator : AbstractValidator<EstateCreateDto>
    {
        public EstateCreateValidator()
        {
            RuleFor(e => e.ConstructionTypeId)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.EstateBoundTypeId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.PostalCode)
                .Length(10).WithMessage("____PostalCode Must 10 Chat");

            RuleFor(e => e.X)
                .MaximumLength(31).WithMessage("X less than 31");

            RuleFor(e => e.Y)
                .MaximumLength(31).WithMessage("Y less than 31");

            RuleFor(e => e.Address)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull")
                .MaximumLength(1023).WithMessage("Address less than 1023");

            RuleFor(e => e.MunipulityId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UsageSellId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UsageConsumtionId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UnitDomesticWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UnitCommercialWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UnitOtherWater)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UnitDomesticSewage)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.UnitOtherSewage)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.EmptyUnit)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.HouseholdNumber)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Premises)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.ImprovementsOverall)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.ImprovementsDomestic)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.ImprovementsCommercial)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.ImprovementsOther)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.ContractualCapacity)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");

            RuleFor(e => e.Storeys)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull");
        }
    }
}
