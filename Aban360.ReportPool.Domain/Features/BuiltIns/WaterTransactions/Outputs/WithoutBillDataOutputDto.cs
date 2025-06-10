namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WithoutBillDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FullName { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public string Address { get; set; }
        public string ZoneTitle { get; set; }

    }
}
