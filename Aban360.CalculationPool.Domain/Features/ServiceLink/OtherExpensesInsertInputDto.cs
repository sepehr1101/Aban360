using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesInsertInputDto
    {
        public string BillId { get; set; }
        public OtherExpensesEnum Offering { get; set; }
        public long Amount { get; set; }
        public OtherExpensesInsertInputDto(string billId,OtherExpensesEnum offering,long amount)
        {
            BillId = billId;
            Offering = offering;
            Amount = amount;
        }
        public OtherExpensesInsertInputDto()
        {
        }
    }
}
