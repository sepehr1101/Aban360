using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;

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
            return isThumbnail ? await _openKmQueryService.GetImageThumbnail(documentId) :
                await _openKmQueryService.GetFileBinary(documentId);
        }
    }
}
