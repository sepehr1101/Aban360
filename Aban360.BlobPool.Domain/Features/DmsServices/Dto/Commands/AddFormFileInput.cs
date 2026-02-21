using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands
{
    public record AddFormFileInput
    {
        public IFormFile File { get; set; } = default!;
        public int DocumentTypeId { get; set; }
        public string? BillId { get; set; }
        public long? TrackNumber { get; set; }
    }
}
