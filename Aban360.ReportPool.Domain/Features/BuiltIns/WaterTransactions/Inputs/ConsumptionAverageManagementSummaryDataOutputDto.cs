namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementSummaryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public string RegionTitle { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }

        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }

        public int BillCount { get; set; }
        public float  ConsumptionAverage { get; set; }
        public float  Consumption { get; set; }
    }
}