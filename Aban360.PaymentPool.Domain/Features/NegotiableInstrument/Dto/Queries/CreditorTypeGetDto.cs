using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries
{
    public record CreditorTypeGetDto
    {
        public CreditorTypeEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }
    }
}
