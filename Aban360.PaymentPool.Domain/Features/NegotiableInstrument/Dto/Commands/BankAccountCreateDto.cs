namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record BankAccountCreateDto
    {
        public short BankId { get; set; }
        public string Title { get; set; } = null!;
        public short AccountTypeId { get; set; }
        public int ZoneId { get; set; }
    }
}
