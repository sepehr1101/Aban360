namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record DeductionsAndDiscountsReportInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
