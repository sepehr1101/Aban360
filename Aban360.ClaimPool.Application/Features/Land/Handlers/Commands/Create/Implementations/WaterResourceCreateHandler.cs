using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    public class WaterResourceCreateHandler : IWaterResourceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterResourceCommandService _waterResourceCommandService;
        private readonly IHeadquartersAddhoc _headquartersAddhoc;
        public WaterResourceCreateHandler(
            IMapper mapper,
            IWaterResourceCommandService WaterResourceCommandService,
            IHeadquartersAddhoc headquartersAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterResourceCommandService = WaterResourceCommandService;
            _waterResourceCommandService.NotNull(nameof(_waterResourceCommandService));

            _headquartersAddhoc = headquartersAddhoc;
            _headquartersAddhoc.NotNull(nameof(_headquartersAddhoc));
        }

        public async Task Handle(WaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            var waterResource = _mapper.Map<WaterResource>(createDto);
            if (waterResource == null)
            {
                throw new InvalidDataException();
            }
            var headquartersTitle=await _headquartersAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
            waterResource.HeadquartersTitle = headquartersTitle;
            await _waterResourceCommandService.Add(waterResource);
        }
    }
}
