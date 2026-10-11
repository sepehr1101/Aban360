namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record DetailGetDto
    {
        public Guid Id { get; set; }
        public Guid TrackingId { get; set; }
        public long TrackNumber { get; set; }
        public string BillId { get; set; } = null!;
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string SureName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Plaque { get; set; } = null!;
        public int UsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int? MeterDiameterId { get; set; }
        public int? SiphonDiameterId { get; set; }
        public string? PostalCode { get; set; }
        public int? PreNumber { get; set; }
        public string? PreDateJalali { get; set; }
        public DateTime? PreDateTime { get; set; }
        public double? PreConsumptionAverage { get; set; }
        public int? PreCounterStateCode { get; set; }
        public string? BodySerial { get; set; }
        public string MeterInstallationDateJalali { get; set; } = null!;
        public string? ChangeDateJalali { get; set; }
        public int? ChangeNumber { get; set; }
        public int? ContractualCapacity { get; set; }
        public string? MobileNumber { get; set; }
        public int? DeletionStateId { get; set; }
        public int? BranchTypeId { get; set; }
        public int? CounterNumber { get; set; }
        public int? CurrentCounterStateCode { get; set; }
        public string? PossibleAddress { get; set; }
        public string? PossibleBodySerial { get; set; }
        public string? PossibleReadingNumber { get; set; }
        public string? PossibleMobileNumber { get; set; }
        public string? PossiblePhoneNumber { get; set; }
        public int? PossibleDomesticUnit { get; set; }
        public int? PossibleCommercialUnit { get; set; }
        public int? PossibleOtherUnit { get; set; }
        public int? PossibleUsageId { get; set; }
        public int? PossibleEmptyUnit { get; set; }
        public DateTime? OffLoadDateTime { get; set; }
        public string? OffloadDateJalali { get; set; }
        public string? OffLoadTime { get; set; }
        public Guid? OffLoadBy { get; set; }
        public string? Y { get; set; }
        public string? X { get; set; }
        public string? GisAccuracy { get; set; }
        public int? ImageCount { get; set; }
        public int? CurrentConsumption { get; set; }
        public double? CurrentConsumptionAverage{ get; set; }
        public int? DateDifference { get; set; }
        public bool? CounterNumberShown { get; set; }
        public bool? ExcludedForBazdid { get; set; }
        public bool? ExcludedForEslah { get; set; }
        public string? JsonItems { get; set; }
        public int? ConsumptionStateId { get; set; }
        public string? Description { get; set; }
        public bool? IsCalculated { get; set; }
        public string? CalculationErrorDescription { get; set; }
        public string? D1 { get; set; }
        public string? D2 { get; set; }
        public int? CounterStatePosition { get; set; }
        public int? AttemptCount { get; set; }
        public bool? IsLocked { get; set; }
        public string? LocationDateTime { get; set; }
        public string? PhoneDateTime { get; set; }
        public string? FatherName { get; set; }
        public string? OldReadingNumber { get; set; }
        public string? OldCustomerNumber { get; set; }
        public string? MobilesNumber { get; set; }
        public long WaterDebt { get; set; }
        public Guid? ModifierUserId { get; set; }
        public int? GuildId { get; set; }
        public string? SiphonInstallationDateJalali { get; set; }
        public int? PreGuildId { get; set; }
        public int? HighLowStateId { get; set; }
        public Guid? ExcludeBy { get; set; }
        public DateTime? ExcludeDateTime { get; set; }
        public int? ExcludeCauseId { get; set; }
        public string? ExcludeCauseTitle { get; set; }
    }
}
