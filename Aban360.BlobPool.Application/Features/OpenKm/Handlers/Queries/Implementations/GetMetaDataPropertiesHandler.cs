using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetMetaDataPropertiesHandler : IGetMetaDataPropertiesHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        private readonly IOpenKmMetaDataQueryServices _openKmMetaDataQueryServices;
        public GetMetaDataPropertiesHandler(
            IOpenKmQueryService openKmQueryService,
            IOpenKmMetaDataQueryServices openKmMetaDataQueryServices)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));

            _openKmMetaDataQueryServices = openKmMetaDataQueryServices;
            _openKmMetaDataQueryServices.NotNull(nameof(openKmMetaDataQueryServices));
        }

        public async Task<ICollection<MetaDataOutput>> Handle(string documentId, CancellationToken cancellationToken)
        {
            MetaDataProperties result = await _openKmQueryService.GetMetaDataProperties(documentId);
            IEnumerable<OpenKmMetaData> openKmMetadata = await _openKmMetaDataQueryServices.Get();

            ICollection<MetaDataOutput> metaDataOutput= GetMetaDataOutput(result, openKmMetadata);
            return metaDataOutput;
        }

        private ICollection<MetaDataOutput> GetMetaDataOutput(MetaDataProperties properties, IEnumerable<OpenKmMetaData> openKmMetadatas)
        {
            ICollection<MetaDataOutput> result = new List<MetaDataOutput>();

            properties.RawMetaDatas.ForEach(metadata =>
            {
                OpenKmMetaData? openKmMetaDataResult = openKmMetadatas
                .Where(openKm => openKm.Section == metadata.Name && openKm.Value == metadata.Value)
                .FirstOrDefault();

                if (openKmMetaDataResult != null)
                {
                    result.Add(new MetaDataOutput(openKmMetaDataResult.KeyLabel, openKmMetaDataResult.Label));
                }
            });
            return result;
        }
    }
}
