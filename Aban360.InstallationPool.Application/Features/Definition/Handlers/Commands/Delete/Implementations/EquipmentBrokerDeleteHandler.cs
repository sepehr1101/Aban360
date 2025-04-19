using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Implementations
{
    internal sealed class EquipmentBrokerDeleteHandler : IEquipmentBrokerDeleteHandler
    {
        private readonly IEquipmentBrokerCommandService _equipmentBrokerCommandService;
        private readonly IEquipmentBrokerQueryService _equipmentBrokerQueryService;
        public EquipmentBrokerDeleteHandler(
            IEquipmentBrokerCommandService equipmentBrokerCommandService,
            IEquipmentBrokerQueryService equipmentBrokerQueryService)
        {
            _equipmentBrokerCommandService = equipmentBrokerCommandService;
            _equipmentBrokerCommandService.NotNull(nameof(_equipmentBrokerCommandService));

            _equipmentBrokerQueryService = equipmentBrokerQueryService;
            _equipmentBrokerQueryService.NotNull(nameof(_equipmentBrokerQueryService));
        }

        public async Task Handle(EquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var equipmentBroker = await _equipmentBrokerQueryService.Get(deleteDto.Id);
            await _equipmentBrokerCommandService.Remove(equipmentBroker);
        }
    }
}
