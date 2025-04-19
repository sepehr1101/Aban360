using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts
{
    public interface IEquipmentBrokerCreateHandler
    {
        Task Handle(EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken);
    }
}
