namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ModifiedBillsDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string UsageSellTitle { get; set; }
        public string Date { get; set; }
        public long InstallationAmount { get; set; }
        public long ServiceLinkAmount { get; set; }
        public long PreparationAmout { get; set; }
        public long ProvincePreparationAmount { get; set; }
        public long OtherAmount { get; set; }
        public long ImpureAmount { get; set; }
        public long OverdueAmount { get; set; }
        public long SumAmount { get; set; }

    }
}
