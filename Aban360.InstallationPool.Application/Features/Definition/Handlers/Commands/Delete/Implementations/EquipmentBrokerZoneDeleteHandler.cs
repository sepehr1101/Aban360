using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Implementations
{
    internal sealed class EquipmentBrokerZoneDeleteHandler : IEquipmentBrokerZoneDeleteHandler
    {
        private readonly IEquipmentBrokerZoneCommandService _equipmentBrokerZoneCommandService;
        private readonly IEquipmentBrokerZoneQueryService _equipmentBrokerZoneQueryService;
        public EquipmentBrokerZoneDeleteHandler(
            IEquipmentBrokerZoneCommandService equipmentBrokerZoneCommandService,
            IEquipmentBrokerZoneQueryService equipmentBrokerZoneQueryService)
        {
            _equipmentBrokerZoneCommandService = equipmentBrokerZoneCommandService;
            _equipmentBrokerZoneCommandService.NotNull(nameof(_equipmentBrokerZoneCommandService));

            _equipmentBrokerZoneQueryService = equipmentBrokerZoneQueryService;
            _equipmentBrokerZoneQueryService.NotNull(nameof(_equipmentBrokerZoneQueryService));
        }

        public async Task Handle(EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(deleteDto.Id);
            await _equipmentBrokerZoneCommandService.Remove(equipmentBrokerZone);
        }
    }
}
