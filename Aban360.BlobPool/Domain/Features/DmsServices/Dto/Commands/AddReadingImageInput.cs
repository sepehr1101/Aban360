using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands
{
    public sealed class AddReadingImageInput
    {
        public string BillId { get; set; } = default!;
        public IFormFile File { get; set; } = default!;
    }
}
