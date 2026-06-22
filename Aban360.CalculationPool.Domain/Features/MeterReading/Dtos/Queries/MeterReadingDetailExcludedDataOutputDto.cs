using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailExcludedDataOutputDto
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
        public string CurrentCounterStateTitle { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }

        public DateTime? ExcludedDateTime { get; set; }
        public Guid? ExcludedByUserId { get; set; }
        public string? ExcludedDateJalali { get { return ExcludedDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }

        public DateTime InsertDateTime { get; set; }
        public Guid InsertByUserId { get; set; }
        public string InsertDateJalali { get { return InsertDateTime.ToShortPersianDateTimeString(); } }

        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int UsageConsumptionId { get; set; }
        public string UsageConsumptionTitle { get; set; }
        public string CommercialUnit { get; set; }
        public string DomesticUnit { get; set; }
        public string OtherUnit { get; set; }
        public string TavizDateJalali { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
    }
}
