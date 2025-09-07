using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetDownloadLinkHandler : IGetDownloadLinkHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public GetDownloadLinkHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<string> Handle(string uuid, bool oneTimeUse, CancellationToken cancellationToken)
        {
            string result = await _openKmQueryService.GetDownloadLink(uuid, oneTimeUse);
            return result;
        }
    }
}
