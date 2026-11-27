using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Contracts
{
    public interface ISewageEquipmentBrokerZoneQueryService
    {
        Task<SewageEquipmentBrokerZone> Get(short id);
        Task<ICollection<SewageEquipmentBrokerZone>> Get();
    }
}
