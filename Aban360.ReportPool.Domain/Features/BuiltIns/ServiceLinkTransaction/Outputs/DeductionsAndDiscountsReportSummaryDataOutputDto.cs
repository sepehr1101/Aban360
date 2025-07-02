namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record DeductionsAndDiscountsReportSummaryDataOutputDto
    {
        public string DiscountTypeTitle { get; set; }
        public long SumOffAmount { get; set; }
    }
}
