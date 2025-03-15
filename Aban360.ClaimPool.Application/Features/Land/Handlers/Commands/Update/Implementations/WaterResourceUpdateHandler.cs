using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public class WaterResourceUpdateHandler : IWaterResourceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterResourceQueryService _waterResourceQueryService;

        private readonly IHeadquartersAddhoc _headquartersAddhoc;
        public WaterResourceUpdateHandler(
            IMapper mapper,
            IWaterResourceQueryService WaterResourceQueryService,
             IHeadquartersAddhoc headquartersAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterResourceQueryService = WaterResourceQueryService;
            _waterResourceQueryService.NotNull(nameof(_waterResourceQueryService));

            _headquartersAddhoc = headquartersAddhoc;
            _headquartersAddhoc.NotNull(nameof(_headquartersAddhoc));
        }

        public async Task Handle(WaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var waterResource = await _waterResourceQueryService.Get(updateDto.Id);
         
            var headquartersTitle = await _headquartersAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
            waterResource.HeadquartersTitle = headquartersTitle;
            
            _mapper.Map(updateDto, waterResource);
        }
    }
}
