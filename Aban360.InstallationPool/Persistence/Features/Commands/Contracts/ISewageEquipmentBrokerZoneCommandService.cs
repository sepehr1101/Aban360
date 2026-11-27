using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Contracts
{
    public interface ISewageEquipmentBrokerZoneCommandService
    {
        Task Add(SewageEquipmentBrokerZone sewageEquipmentBrokerZone);
        Task Remove(SewageEquipmentBrokerZone sewageEquipmentBrokerZone);
    }
}
