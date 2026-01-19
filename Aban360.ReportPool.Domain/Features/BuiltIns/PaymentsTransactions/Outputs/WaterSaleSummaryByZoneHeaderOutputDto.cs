namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterSaleSummaryByZoneHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public bool IsNet { get; set; }
        
        public int BillCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }
    }
}
