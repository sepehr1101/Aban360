using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailCheckedDto
    {
        public int Id { get; set; }
        public int FlowImportedId { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public int AgentCode { get; set; }
        public short CurrentCounterStateCode { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }

        //public Guid? ExcludedByUserId { get; set; }
        //public DateTime? ExcludedDateTime { get; set; }

        public Guid InsertByUserId { get; set; }
        public DateTime InsertDateTime { get; set; }
        //public Guid? RemovedByUserId { get; set; }
        //public DateTime? RemovedDateTime { get; set; }

        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle {  get; set; }
        public int ConsumptionUsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }
        public string WaterRegisterDate { get; set; }
        public string SewageRegisterDate { get; set; }
        public int WaterCount { get; set; }
        public int SewageCalcState { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDate { get; set; }
        public string? VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public short MeterDiameterId { get; set; }
        public int VirtualCategoryId { get; set; }
        public string? BodySerial { get; set; }

        public string? TavizDateJalali { get; set; }
        public string? TavizCause { get; set; }
        public string? TavizRegisterDateJalali { get; set; }
        public int? TavizNumber { get; set; }

        //previous
        public string LastMeterDateJalali { get; set; }
        public int? LastMeterNumber { get; set; }
        public float? LastConsumption { get; set; }
        public float? LastMonthlyConsumption { get; set; }
        public int? LastCounterStateCode { get; set; }
        public double? LastSumItems { get; set; }

        //current
        public double? SumItems { get; set; }
        public double? SumItemsBeforeDiscount { get; set; }
        public double? DiscountSum { get; set; }

        public double? Consumption { get; set; }
        public double? MonthlyConsumption { get; set; }

        public HighLowEnum AttentionState { get; set; }
    }
}
