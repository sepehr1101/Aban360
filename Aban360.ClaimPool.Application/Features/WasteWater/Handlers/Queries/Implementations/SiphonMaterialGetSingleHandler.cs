using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class SiphonMaterialGetSingleHandler : ISiphonMaterialGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonMaterialQueryService _queryService;
        public SiphonMaterialGetSingleHandler(
            IMapper mapper,
            ISiphonMaterialQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<SiphonMaterialGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            SiphonMaterial siphonMaterial = await _queryService.Get(id);
            return _mapper.Map<SiphonMaterialGetDto>(siphonMaterial);
        }
    }
}
