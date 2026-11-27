using Microsoft.AspNetCore.Http;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record CreditWithDocumentCreateDto
    {
        //CreditByDocumentCreateDto
        public string? LetterNumber { get; set; }
        public short BankId { get; set; }

        //DocumentObjectCreateDto
        public IFormFile DocumentFile { get; set; } = default!;
        public short DocumentTypeId { get; set; }
        public string? Description { get; set; }
    }
}

