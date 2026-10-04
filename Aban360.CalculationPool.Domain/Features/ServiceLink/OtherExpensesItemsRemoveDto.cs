namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsRemoveDto
    {
        public int Id { get; set; }
        public DateTime RemoveDateTime { get; set; }
        public Guid RemoveBy { get; set; }
    }
}
