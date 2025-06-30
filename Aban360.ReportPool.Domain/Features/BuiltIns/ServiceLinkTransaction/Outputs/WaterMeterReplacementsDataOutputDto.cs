namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record WaterMeterReplacementsDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string  FullName{ get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string MeterChangeDate { get; set; }
        public string RegistrationDate { get; set; }
        public string MeterSerial { get; set; }
        public string ZoneTitle { get; set; }
    }
}
