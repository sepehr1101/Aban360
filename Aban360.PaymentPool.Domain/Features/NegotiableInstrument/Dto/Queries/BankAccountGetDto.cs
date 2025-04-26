using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries
{
    public record BankAccountGetDto
    {
        public short Id { get; set; }
        public short BankId { get; set; }
        public string Title { get; set; } = null!;
        public short AccountTypeId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
    }
}
