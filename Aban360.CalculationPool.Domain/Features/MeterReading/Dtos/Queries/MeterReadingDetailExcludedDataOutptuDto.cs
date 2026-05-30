namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailExcludedDataOutptuDto
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int FlowImportedId { get; set; }
        public string ReadingNumber { get; set; }
        public int CurrentCounterStateCode { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string ExcludedDateTime { get; set; }
        public Guid ExcludedByUserId { get; set; }
        public Guid InsertByUserId { get; set; }
        public string InsertDateTime { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int ConsumptionUsageId { get; set; }
        public string ConsumptionUsageTitle { get; set; }
        public string CommercialUnit { get; set; }
        public string DomesticUnit { get; set; }
        public string OtherUnit { get; set; }
        public string TavizDateJalali { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
    }
}
