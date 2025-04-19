using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class EquipmentBrokerZoneCreateHandler : IEquipmentBrokerZoneCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerZoneCommandService _equipmentBrokerZoneCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        public EquipmentBrokerZoneCreateHandler(
            IMapper mapper,
            IEquipmentBrokerZoneCommandService equipmentBrokerZoneCommandService,
            IZoneTitleAddhoc zoneTitleAddHhock)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerZoneCommandService = equipmentBrokerZoneCommandService;
            _equipmentBrokerZoneCommandService.NotNull(nameof(_equipmentBrokerZoneCommandService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));
        }

        public async Task Handle(EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            var equipmentBrokerZone = _mapper.Map<EquipmentBrokerZone>(createDto);
            equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(createDto.ZoneId,cancellationToken);
            await _equipmentBrokerZoneCommandService.Add(equipmentBrokerZone);
        }
    }
}
