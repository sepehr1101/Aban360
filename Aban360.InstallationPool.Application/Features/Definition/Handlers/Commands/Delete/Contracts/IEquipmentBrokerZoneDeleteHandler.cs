using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts
{
    public interface IEquipmentBrokerZoneDeleteHandler
    {
        Task Handle(EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
