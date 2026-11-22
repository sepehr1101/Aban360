using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Repositories
{
    internal sealed class EquipmentBrokerRepository : IEquipmentBrokerCommandService, IEquipmentBrokerQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBroker> _equipmentBroker;
        public EquipmentBrokerRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _equipmentBroker = _uow.Set<EquipmentBroker>();
            _equipmentBroker.NotNull(nameof(_equipmentBroker));
        }

        public async Task Add(EquipmentBroker equipmentBroker)
        {
            await _equipmentBroker.AddAsync(equipmentBroker);
        }

        public async Task Remove(EquipmentBroker equipmentBroker)
        {
            _equipmentBroker.Remove(equipmentBroker);
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
