using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Implementations
{
    internal sealed class EquipmentBrokerQueryService : IEquipmentBrokerQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBroker> _equipmentBroker;
        public EquipmentBrokerQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _equipmentBroker = _uow.Set<EquipmentBroker>();
            _equipmentBroker.NotNull(nameof(_equipmentBroker));
        }

        public async Task<EquipmentBroker> Get(short id)
        {
            return await _uow.FindOrThrowAsync<EquipmentBroker>(id);
        }

        public async Task<ICollection<EquipmentBroker>> Get()
        {
            return await _equipmentBroker.ToListAsync();
        }
    }
}
