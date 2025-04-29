namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record CreditByDocumentCreateDto
    {
        public Guid DocumentId { get; set; }
        public string? LetterNumber { get; set; }
        public short BankId { get; set; }
    }
}
