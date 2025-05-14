using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Contracts
{
    public interface IEquipmentBrokerZoneCommandService
    {
        Task Add(EquipmentBrokerZone equipmentBrokerZone);
        Task Remove(EquipmentBrokerZone equipmentBrokerZone);

    }
}