using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsDataDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public long Amount { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateJalai { get { return InsertDateTime.ToShortPersianDateString(); } }
        public Guid RemoveBy { get; set; }
        public DateTime? RemoveDateTime { get; set; }
        public string? RemoveDateJalali { get { return RemoveDateTime?.ToShortPersianDateString() ?? string.Empty; } }
    }
}
