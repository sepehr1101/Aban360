namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record DeductionsAndDiscountsReportDetailDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string DisCountTypeTitle { get; set; }
        public string Offering { get; set; }//todo
        public long OfferingAmount { get; set; }
        public long TotalAmount { get; set; }//todo

        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
    
    }  public record DeductionsAndDiscountsReportSummaryDataOutputDto
    {
        public string DisCountTypeTitle { get; set; }
        public long OfferingAmount { get; set; }
        public long TotalAmount { get; set; }//todo

        public string ZoneTitle { get; set; }
    }
}
