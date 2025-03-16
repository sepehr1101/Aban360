using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class EstateWaterResourceGetSingleHandler : IEstateWaterResourceGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceQueryService _estateWaterResourceQueryService;
        public EstateWaterResourceGetSingleHandler(
            IMapper mapper,
            IEstateWaterResourceQueryService EstateWaterResourceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceQueryService = EstateWaterResourceQueryService;
            _estateWaterResourceQueryService.NotNull(nameof(_estateWaterResourceQueryService));
        }

        public async Task<EstateWaterResourceGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            EstateWaterResource estateWaterResource = await _estateWaterResourceQueryService.Get(id);
            return _mapper.Map<EstateWaterResourceGetDto>(estateWaterResource);
        }
    }
}
