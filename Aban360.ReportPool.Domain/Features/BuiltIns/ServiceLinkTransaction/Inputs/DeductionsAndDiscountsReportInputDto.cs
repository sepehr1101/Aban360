namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record DeductionsAndDiscountsReportInputDto
    {
        //Other
        public ICollection<int> ZoneIds { get; set; }
    }
}
