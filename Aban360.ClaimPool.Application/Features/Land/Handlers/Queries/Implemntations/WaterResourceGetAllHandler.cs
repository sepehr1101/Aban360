using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class WaterResourceGetAllHandler : IWaterResourceGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterResourceQueryService _waterResourceQueryService;
        public WaterResourceGetAllHandler(
            IMapper mapper,
            IWaterResourceQueryService WaterResourceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterResourceQueryService = WaterResourceQueryService;
            _waterResourceQueryService.NotNull(nameof(_waterResourceQueryService));
        }

        public async Task<ICollection<WaterResourceGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<WaterResource> waterResource = await _waterResourceQueryService.Get();
            return _mapper.Map<ICollection<WaterResourceGetDto>>(waterResource);
        }
    }
}
