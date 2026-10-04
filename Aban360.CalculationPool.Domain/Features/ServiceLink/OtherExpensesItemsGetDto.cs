using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsGetDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public long Amount { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateJalai { get { return InsertDateTime.ToShortPersianDateString(); } }
        public DateTime? RemoveDateTime { get; set; }
        public string? RemoveDateJalali { get { return RemoveDateTime?.ToShortPersianDateString() ?? string.Empty; } }
    }
}
