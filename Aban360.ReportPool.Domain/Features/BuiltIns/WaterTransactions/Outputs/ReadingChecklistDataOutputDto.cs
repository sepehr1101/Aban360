namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingChecklistDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int DomesticUnit { get; set; }
        public int NonDomesticUnit{ get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public string LastCounterStateCode { get; set; }
        public string CustomerNumber { get; set; }

    }
}
