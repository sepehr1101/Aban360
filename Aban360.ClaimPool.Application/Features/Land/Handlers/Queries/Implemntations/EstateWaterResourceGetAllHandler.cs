using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class EstateWaterResourceGetAllHandler : IEstateWaterResourceGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceQueryService _estateWaterResourceQueryService;
        public EstateWaterResourceGetAllHandler(
            IMapper mapper,
            IEstateWaterResourceQueryService EstateWaterResourceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceQueryService = EstateWaterResourceQueryService;
            _estateWaterResourceQueryService.NotNull(nameof(_estateWaterResourceQueryService));
        }

        public async Task<ICollection<EstateWaterResourceGetDto>> Handle(CancellationToken cancellationToken)
        {
            var estateWaterResource = await _estateWaterResourceQueryService.Get();
            return _mapper.Map<ICollection<EstateWaterResourceGetDto>>(estateWaterResource);
        }
    }
}
