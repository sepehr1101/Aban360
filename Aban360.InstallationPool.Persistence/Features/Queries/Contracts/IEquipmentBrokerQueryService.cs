using Aban360.InstallationPool.Domain.Features.Definition.Entities;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Contracts
{
    public interface IEquipmentBrokerQueryService
    {
        Task<EquipmentBroker> Get(short id);
        Task<ICollection<EquipmentBroker>> Get();
    }}
