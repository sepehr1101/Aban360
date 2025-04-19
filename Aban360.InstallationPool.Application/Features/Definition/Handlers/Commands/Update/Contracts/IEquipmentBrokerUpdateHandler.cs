using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts
{
    public interface IEquipmentBrokerUpdateHandler
    {
        Task Handle(EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
