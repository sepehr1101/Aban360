using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using System.Drawing;
using System.Drawing.Imaging;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetDocumentBinaryHandler : IGetDocumentBinaryHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public GetDocumentBinaryHandler(
            IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(_openKmQueryService));
        }
        public async Task<byte[]> Handle(string documentId, bool isThumbnail, CancellationToken cancellationToken)
        {
            byte[] image= isThumbnail ? await _openKmQueryService.GetImageThumbnail(documentId) :
                await _openKmQueryService.GetFileBinary(documentId);
            if (IsTiff(image))
            {
                using var inputStream = new MemoryStream(image);
                using var bitmap = new Bitmap(inputStream);

                using var outputStream = new MemoryStream();
                bitmap.Save(outputStream, ImageFormat.Jpeg);

                return outputStream.ToArray();
            }
            return image;
        }
        private bool IsTiff(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return false;

            return
                // Little-endian: "II" 2A 00
                (bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00) ||

                // Big-endian:    "MM" 00 2A
                (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A);
        }
    }
}
