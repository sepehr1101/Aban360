using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record CreditWithoutDocumentCreateDto
    {
        public string PaymentId{ get; set; }
        public CreditorTypeEnum CreditorTypeId { get; set; }
        public short BankId { get; set; }

    }
}
