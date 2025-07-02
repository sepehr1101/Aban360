namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record DeductionsAndDiscountsReportHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDate { get; set; }
        public int RecordCount { get; set; }

        public long TotalMeterAmount { get; set; }
        public long TotalSewageAmount { get; set; }
        public long TotalSumAmount { get; set; }

    }
}
