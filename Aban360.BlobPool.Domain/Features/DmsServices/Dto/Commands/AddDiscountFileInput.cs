using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands
{
    public record AddDiscountFileInput
    {
        public IFormFile File { get; set; } = default!;
        public int DocumentTypeId { get; set; }
        public string Id { get; set; }= default!;
    }
}
