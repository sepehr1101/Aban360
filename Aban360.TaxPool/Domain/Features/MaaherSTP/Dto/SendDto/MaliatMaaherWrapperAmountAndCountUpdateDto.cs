namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaliatMaaherWrapperAmountAndCountUpdateDto
    {
        public int Id { get; set; }
        public int InvoiceCount { get; set; }
        public long SumAmount { get; set; }
        public MaliatMaaherWrapperAmountAndCountUpdateDto(int id, int invoiceCount, long sumAmount)
        {
            Id = id;
            InvoiceCount = invoiceCount;
            SumAmount = sumAmount;
        }
    }
}
