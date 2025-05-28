namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record FlatInfoDto
    {
        public int FlatNumber { get; set; }
        public string UsageTitle { get; set; }
        public string UsageConsumptionTitle { get; set; }
        public string GuildTitle { get; set; }
     
        public short ContractualCapacity { get; set; }
        public string CapacityCalculationIndexTitle { get; set; }
        public string CapacityCalculationIndexValue { get; set; }
     
        public short ImprovementsOverall { get; set; }
        public bool EmptyUnit { get; set; }

    }    
}
