namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WithoutBillHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        //public long FromAmount { get; set; }
        //public long ToAmount { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

    }
}
