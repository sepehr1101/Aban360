using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Repositories
{
    internal sealed class SewageEquipmentBrokerRepository : ISewageEquipmentBrokerCommandService, ISewageEquipmentBrokerQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SewageEquipmentBroker> _sewageEquipmentBroker;
        public SewageEquipmentBrokerRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _sewageEquipmentBroker = _uow.Set<SewageEquipmentBroker>();
            _sewageEquipmentBroker.NotNull(nameof(_sewageEquipmentBroker));
        }

        public async Task Add(SewageEquipmentBroker sewageEquipmentBroker)
        {
            await _sewageEquipmentBroker.AddAsync(sewageEquipmentBroker);
        }

        public async Task Remove(SewageEquipmentBroker sewageEquipmentBroker)
        {
            _sewageEquipmentBroker.Remove(sewageEquipmentBroker);
        }

        public async Task<SewageEquipmentBroker> Get(short id)
        {
            return await _uow.FindOrThrowAsync<SewageEquipmentBroker>(id);
        }

        public async Task<ICollection<SewageEquipmentBroker>> Get()
        {
            return await _sewageEquipmentBroker.ToListAsync();
        }

    }
}
