using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record EstateCreateDto
    {
        public short ConstructionTypeId { get; set; }
        public short EstateBoundTypeId { get; set; }
        public string? PostalCode { get; set; }

        public string? X { get; set; }

        public string? Y { get; set; }

        public string? Parcel { get; set; }

        public string Address { get; set; } = null!;

        public int MunipulityId { get; set; }

        public short UsageSellId { get; set; }

        public short UsageConsumtionId { get; set; }

        public short UnitDomesticWater { get; set; }

        public short UnitCommercialWater { get; set; }

        public short UnitOtherWater { get; set; }

        public short UnitDomesticSewage { get; set; }

        public short UnitCommercialSewage { get; set; }

        public short UnitOtherSewage { get; set; }

        public short EmptyUnit { get; set; }

        public short HouseholdNumber { get; set; }

        public int Premises { get; set; }

        public int ImprovementsOverall { get; set; }

        public int ImprovementsDomestic { get; set; }

        public int ImprovementsCommercial { get; set; }

        public int ImprovementsOther { get; set; }

        public int ContractualCapacity { get; set; }

        public short Storeys { get; set; }

        public Guid UserId { get; set; }

        public int? PreviousId { get; set; }

        public DateTime ValidFrom { get; set; }

    }
}