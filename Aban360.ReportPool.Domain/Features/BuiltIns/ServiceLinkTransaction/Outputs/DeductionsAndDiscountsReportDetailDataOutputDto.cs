namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record DeductionsAndDiscountsReportDetailDataOutputDto
    {
        public string DiscountTypeTitle { get; set; }
        public string OffTypeTitle { get; set; }
        public string DiscountTitle { get; set; }
        public long OffAmount { get; set; }

    }
}
