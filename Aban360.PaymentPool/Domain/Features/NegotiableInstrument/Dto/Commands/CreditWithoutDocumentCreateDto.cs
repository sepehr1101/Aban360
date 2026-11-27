using Aban360.PaymentPool.Domain.Constansts;
using Microsoft.AspNetCore.Http;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record CreditWithoutDocumentCreateDto
    {
        public string PaymentId{ get; set; }
        public CreditorTypeEnum CreditorTypeId { get; set; }
        public short BankId { get; set; }
        public PaymentMethodEnum paymentMethodId { get; set; }

        //DocumentObjectCreateDto
        public IFormFile? DocumentFile { get; set; } 
        public short DocumentTypeId { get; set; }
        public string? Description { get; set; }
    }
}
