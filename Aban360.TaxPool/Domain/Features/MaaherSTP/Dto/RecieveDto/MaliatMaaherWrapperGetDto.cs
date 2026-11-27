namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto
{
    public record MaliatMaaherWrapperGetDto
    {
        public int Id { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid InsertByUserId { get; set; }
        public DateTime ConfirmedDateTime { get; set; }
        public DateTime SendDateTime { get; set; }
        public Guid SendByUserId { get; set; }
        public int InvoiceCount { get; set; }
        public long SumAmount { get; set; }
    }
}
