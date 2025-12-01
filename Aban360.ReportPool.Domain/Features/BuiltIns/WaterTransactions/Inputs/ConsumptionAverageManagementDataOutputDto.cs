namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string CustomerNumber { get; set; }
        public string RegisterDayJalali { get; set; }

        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string UsageTitle { get; set; }
        public string BranchTypeTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }

        public float  ConsumptionAverage { get; set; }
        public float  Consumption { get; set; }
    }
}