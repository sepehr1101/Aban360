namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record UseStateReportDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string EventDateJalali { get; set; }
        public string DeptAmount { get; set; }
        public string Address { get; set; }
        public string ZoneTitle { get; set; }

    }
}