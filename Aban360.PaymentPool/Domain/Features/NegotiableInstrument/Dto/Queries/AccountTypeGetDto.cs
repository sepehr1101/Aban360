using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries
{
    public record AccountTypeGetDto
    {
        public AccountTypeEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
