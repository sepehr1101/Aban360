namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageAnalysisInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<ContractualAndOlgooLevelValuesInputDto> Values { get; set; }
        public bool IsZoneGroup { get; set; }
    }
}
