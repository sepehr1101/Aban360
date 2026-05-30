namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerEstateUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }

        public int GuildId { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }

        public int Premises { get; set; }
        public int ImprovementCommertial { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementOverall { get; set; }

        public int UsageSellId { get; set; }
        public int UsageConsumptionId { get; set; }
        public int EmptyUnit { get; set; }
        public int ContractualCapacity { get; set; }
    }
}
