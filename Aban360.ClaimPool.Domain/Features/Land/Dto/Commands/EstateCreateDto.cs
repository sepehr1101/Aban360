namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record EstateCreateDto
    {
        public short ConstructionTypeId { get; set; }

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

        public short Premises { get; set; }

        public short ImprovementsOverall { get; set; }

        public short ImprovementsDomestic { get; set; }

        public short ImprovementsCommercial { get; set; }

        public short ImprovementsOther { get; set; }

        public short ContractualCapacity { get; set; }

        public short Storeys { get; set; }

        public Guid UserId { get; set; }

        public int? PreviousId { get; set; }

        public DateTime ValidFrom { get; set; }

    }
}