using Microsoft.AspNetCore.Http;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record DocumentObjectCreateDto
    {
        public IFormFile DocumentFile { get; set; } = default!;
        public short DocumentTypeId { get; set; }
        public Guid? ParrentId { get; set; }
        public string? Description { get; set; }
    }
}
