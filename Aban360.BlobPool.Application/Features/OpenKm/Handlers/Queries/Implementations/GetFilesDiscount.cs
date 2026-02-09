using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetFilesDiscount
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public GetFilesDiscount(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<FileListResponse> Handle(string input, CancellationToken cancellationToken)
        {
            FileListResponse result = await _openKmQueryService.GetFilesDiscount(input);
            return result;
        }
    }
}
