namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsInsertInputDto
    {
        public int ItemId { get; set; }
        public long Amount { get; set; }
    }
}
