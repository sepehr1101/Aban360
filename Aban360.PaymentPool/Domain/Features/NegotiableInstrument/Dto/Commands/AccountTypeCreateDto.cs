using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record AccountTypeCreateDto
    {
        public AccountTypeEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
