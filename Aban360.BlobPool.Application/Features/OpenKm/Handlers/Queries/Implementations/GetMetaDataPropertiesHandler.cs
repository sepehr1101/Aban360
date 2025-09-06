using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetMetaDataPropertiesHandler : IGetMetaDataPropertiesHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public GetMetaDataPropertiesHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<MetaDataProperties> Handle(string documentId, CancellationToken cancellationToken)
        {
            MetaDataProperties result = await _openKmQueryService.GetMetaDataProperties(documentId);
            return result;
        }
    }
}
