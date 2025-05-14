using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Implementations
{
    internal sealed class SewageEquipmentBrokerDeleteHandler : ISewageEquipmentBrokerDeleteHandler
    {
        private readonly ISewageEquipmentBrokerCommandService _sewageEquipmentBrokerCommandService;
        private readonly ISewageEquipmentBrokerQueryService _sewageEquipmentBrokerQueryService;
        public SewageEquipmentBrokerDeleteHandler(
            ISewageEquipmentBrokerCommandService sewageEquipmentBrokerCommandService,
            ISewageEquipmentBrokerQueryService sewageEquipmentBrokerQueryService)
        {
            _sewageEquipmentBrokerCommandService = sewageEquipmentBrokerCommandService;
            _sewageEquipmentBrokerCommandService.NotNull(nameof(_sewageEquipmentBrokerCommandService));

            _sewageEquipmentBrokerQueryService = sewageEquipmentBrokerQueryService;
            _sewageEquipmentBrokerQueryService.NotNull(nameof(_sewageEquipmentBrokerQueryService));
        }

        public async Task Handle(SewageEquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var sewageEquipmentBroker = await _sewageEquipmentBrokerQueryService.Get(deleteDto.Id);
            await _sewageEquipmentBrokerCommandService.Remove(sewageEquipmentBroker);
        }
    }
}
