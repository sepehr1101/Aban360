namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ReadingIssueDistanceBillDataOutputDto
    {
        public int RegionId { get; set; }
        public int ZoneId { get; set; }
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int NextNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public string CounterStateTitle { get; set; }
        public string UsageTitle { get; set; }
        public string BranchType { get; set; }
        public int WaterDiameterId { get; set; }
        public string WaterDiameterTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string ContractualCapacity { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public string ReadingStateTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public int Distance { get; set; }
        public string DistanceText { get; set; }
    }
}
         