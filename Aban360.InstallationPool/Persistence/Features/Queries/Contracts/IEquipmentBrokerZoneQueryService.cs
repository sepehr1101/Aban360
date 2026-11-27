using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Contracts
{
    public interface IEquipmentBrokerZoneQueryService
    {
        Task<EquipmentBrokerZone> Get(short id);
        Task<ICollection<EquipmentBrokerZone>> Get();
    }}
