using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts
{
    public interface IEquipmentBrokerZoneUpdateHandler
    {
        Task Handle(EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
