using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Contracts
{
    public interface ISewageEquipmentBrokerCommandService
    {
        Task Add(SewageEquipmentBroker sewageEquipmentBroker);
        Task Remove(SewageEquipmentBroker sewageEquipmentBroker);
    }
}
