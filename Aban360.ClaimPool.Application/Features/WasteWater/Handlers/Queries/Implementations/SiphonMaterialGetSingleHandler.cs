using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class SiphonMaterialGetSingleHandler : ISiphonMaterialGetSingleHandler
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
            var siphonMaterial = await _queryService.Get(id);
            if (siphonMaterial == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<SiphonMaterialGetDto>(siphonMaterial);
        }
    }
}
