namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record UploaderUpdateDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public short BankId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public int InsertRecordCount { get; set; }
        public long Amount { get; set; }
        public Guid? DocumentId { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
