namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record BankAccountCreateDto
    {
        public short BankId { get; set; }
        public string Title { get; set; } = null!;
        public short AccountTypeId { get; set; }
        public string IBan { get; set; } = null!;
        public string Number { get; set; } = null!;
        public int RegionId { get; set; }
    }
}
