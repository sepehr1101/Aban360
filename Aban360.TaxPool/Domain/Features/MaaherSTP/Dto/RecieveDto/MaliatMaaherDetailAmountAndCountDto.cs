namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto
{
    public record MaliatMaaherDetailAmountAndCountDto
    {
        public int InvoiceCount { get; set; }
        public long SumAmount { get; set; }
    }
}