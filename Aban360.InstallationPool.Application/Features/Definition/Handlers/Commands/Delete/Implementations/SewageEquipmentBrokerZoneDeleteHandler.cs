using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneDeleteHandler : ISewageEquipmentBrokerZoneDeleteHandler
    {
        private readonly ISewageEquipmentBrokerZoneCommandService _sewageEquipmentBrokerZoneCommandService;
        private readonly ISewageEquipmentBrokerZoneQueryService _sewageEquipmentBrokerZoneQueryService;
        public SewageEquipmentBrokerZoneDeleteHandler(
            ISewageEquipmentBrokerZoneCommandService sewageEquipmentBrokerZoneCommandService,
            ISewageEquipmentBrokerZoneQueryService sewageEquipmentBrokerZoneQueryService)
        {
            _sewageEquipmentBrokerZoneCommandService = sewageEquipmentBrokerZoneCommandService;
            _sewageEquipmentBrokerZoneCommandService.NotNull(nameof(_sewageEquipmentBrokerZoneCommandService));

            _sewageEquipmentBrokerZoneQueryService = sewageEquipmentBrokerZoneQueryService;
            _sewageEquipmentBrokerZoneQueryService.NotNull(nameof(_sewageEquipmentBrokerZoneQueryService));
        }

        public async Task Handle(SewageEquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get(deleteDto.Id);
            await _sewageEquipmentBrokerZoneCommandService.Remove(sewageEquipmentBrokerZone);
        }
    }
}
