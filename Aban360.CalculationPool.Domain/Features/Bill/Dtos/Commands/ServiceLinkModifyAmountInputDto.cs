namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ServiceLinkModifyAmountInputDto
    {
        public string BillId { get; set; }
        public int DebtorOrCreditor { get; set; }
        public int ItemTypeId { get; set; }
        public int DiscountTypeId { get; set; }
        public double Amount { get; set; }
    }
}
