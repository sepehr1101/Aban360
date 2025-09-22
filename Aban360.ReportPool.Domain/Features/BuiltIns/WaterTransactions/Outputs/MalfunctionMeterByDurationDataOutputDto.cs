namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterByDurationDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public int ContractualCapacity { get; set; }
        public string MeterInstallationDateJalali { get; set; }
        public string UsageTitle { get; set; }
        public string BranchType { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
        public int DomesticUnit{ get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public float ConsumptionAverage { get; set; }
        public int Consumption { get; set; }
        public int MalfunctionPeriodCount { get; set; }
        public string LastChangeDateJalali { get; set; }
        public string Address { get; set; }
        public string MeterLife { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string MeterRequestDateJalali { get; set; }
        public string DeletionStateTitle { get; set; }
        public long SumItems { get; set; }


    }
}
