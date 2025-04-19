using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Contracts
{
    public interface IEquipmentBrokerCommandService
    {
        Task Add(EquipmentBroker equipmentBroker);
        Task Remove(EquipmentBroker equipmentBroker);
    }
}
