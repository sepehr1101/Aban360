namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ServiceLinkModifyAmountInputDto
    {
        public string BillId { get; set; }
        public int DebtorOrCreditor { get; set; }// debtor:1 creditor:2
        public int ItemTypeId { get; set; }
        public int DiscountTypeId { get; set; }
        public double Amount { get; set; }
    }
}
