namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record UnreadDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FullName { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public long DebtAmount { get; set; }
        public string Address { get; set; }
        public int PeriodCount { get; set; }
        public string ZoneTitle { get; set; }
        public string CounterStateTitle { get; set; }
        public string WaterRequestDateJalali { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ContractualCapacity { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public string UsageTitle { get; set; }
        public string NationalCode { get; set; }
        public string EmptyUnit { get; set; }
        public string BillId { get; set; }
    }
}