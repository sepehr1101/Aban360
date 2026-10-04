namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsInsertDto
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public long Amount { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid? RemoveBy { get; set; } = null;
        public DateTime? RemoveDateTime { get; set; } = null;
    }
}
