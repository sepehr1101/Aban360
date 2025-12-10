namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaliatMaaherWrapperInsertDto
    {
        public DateTime InsertDateTime { get; set; }
        public Guid InsertByUserId { get; set; }
        public int InvoiceCount { get; set; }
        public int SumAmount { get; set; }
        public MaliatMaaherWrapperInsertDto(DateTime insertDateTime, Guid insertByUserId, int invoiceCount, int sumAmount)
        {
            InsertByUserId = insertByUserId;
            InsertDateTime = insertDateTime;
            InvoiceCount = invoiceCount;
            SumAmount = sumAmount;
        }
    }
}
