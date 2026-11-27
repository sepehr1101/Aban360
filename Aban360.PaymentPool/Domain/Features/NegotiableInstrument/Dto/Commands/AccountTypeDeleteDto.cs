using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record AccountTypeDeleteDto
    {
        public AccountTypeEnum Id { get; set; }
    }
}
