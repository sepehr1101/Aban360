using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class EstateWaterResourceUpdateHandler : IEstateWaterResourceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceQueryService _estateWaterResourceQueryService;
        public EstateWaterResourceUpdateHandler(
            IMapper mapper,
            IEstateWaterResourceQueryService EstateWaterResourceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceQueryService = EstateWaterResourceQueryService;
            _estateWaterResourceQueryService.NotNull(nameof(_estateWaterResourceQueryService));
        }

        public async Task Handle(EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            EstateWaterResource estateWaterResource = await _estateWaterResourceQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, estateWaterResource);
        }
    }
}
