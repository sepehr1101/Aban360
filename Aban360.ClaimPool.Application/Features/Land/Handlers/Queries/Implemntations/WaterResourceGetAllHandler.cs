using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class WaterResourceGetAllHandler : IWaterResourceGetAllHandler
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
            var WaterResource = await _waterResourceQueryService.Get();
            return _mapper.Map<ICollection<WaterResourceGetDto>>(WaterResource);
        }
    }
}
