using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class EquipmentBrokerZoneUpdateHandler : IEquipmentBrokerZoneUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerZoneQueryService _equipmentBrokerZoneQueryService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        public EquipmentBrokerZoneUpdateHandler(
            IMapper mapper,
            IEquipmentBrokerZoneQueryService equipmentBrokerZoneQueryService,
            IZoneTitleAddhoc zoneTitleAddHhock)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerZoneQueryService = equipmentBrokerZoneQueryService;
            _equipmentBrokerZoneQueryService.NotNull(nameof(_equipmentBrokerZoneQueryService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));
        }



        public async Task Handle(EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(updateDto.Id);
            equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(updateDto.ZoneId, cancellationToken);
            _mapper.Map(updateDto, equipmentBrokerZone);
        }
    }
}
