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
        public string WaterRegistrationDate { get; set; }
        public string BodySerial { get; set; }
        public string ZoneTitle { get; set; }
        public string ChangeCauseTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }

    }
}
