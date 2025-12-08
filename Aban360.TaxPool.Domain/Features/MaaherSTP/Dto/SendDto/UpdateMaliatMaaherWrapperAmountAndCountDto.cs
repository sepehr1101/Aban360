namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record UpdateMaliatMaaherWrapperAmountAndCountDto
    {
        public int Id { get; set; }
        public int InvoiceCount { get; set; }
        public long SumAmount { get; set; }
        public UpdateMaliatMaaherWrapperAmountAndCountDto(int id, int invoiceCount, long sumAmount)
        {
            Id = id;
            InvoiceCount = invoiceCount;
            SumAmount = sumAmount;
        }
    }
}
