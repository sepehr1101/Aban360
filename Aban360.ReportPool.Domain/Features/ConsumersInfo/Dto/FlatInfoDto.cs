namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record FlatInfoDto
    {
        public int FlatId { get; set; }
        public string UsageTitle { get; set; }
        public string SecondaryWaterUse { get; set; }
        public string BusinessCategory { get; set; }
     
        public short Capacity { get; set; }
        public string CapacityCalculationIndexTitle { get; set; }
        public string CapacityCalculationIndexValue { get; set; }
     
        public short ImprovementUnitArea { get; set; }
        public bool VacancyStatus { get; set; }

    }    
}
