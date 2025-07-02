namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record DeductionsAndDiscountsReportHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDate { get; set; }
        public int RecordCount { get; set; }

        public long TotalOffAmount { get; set; }

    }
}
