using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IReadingImageWatermarker
    {
        Task<byte[]> Apply(IFormFile file, string billId, string uploadedBy, DateTime capturedAt, CancellationToken cancellationToken);
    }
}
