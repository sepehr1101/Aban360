namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterPaymentReceivableHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int? CustomerCount { get; set; }
        public int? BillCount { get; set; }

        public long Amount { get; set; }
        public long OverdueAmount { get; set; }
        public int OverdueCount { get; set; }
        public long DueAmount { get; set; }
        public int DueCount { get; set; }
        public string? Title { get; set; }


    }
}
