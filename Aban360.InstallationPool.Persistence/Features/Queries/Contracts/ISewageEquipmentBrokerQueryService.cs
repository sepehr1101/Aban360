using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Contracts
{
    public interface ISewageEquipmentBrokerQueryService
    {
        Task<SewageEquipmentBroker> Get(short id);
        Task<ICollection<SewageEquipmentBroker>> Get();
    }
}
