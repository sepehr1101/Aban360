using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Implementations
{
    internal sealed class EquipmentBrokerZoneQueryService : IEquipmentBrokerZoneQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBrokerZone> _equipmentBrokerZone;
        public EquipmentBrokerZoneQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _equipmentBrokerZone = _uow.Set<EquipmentBrokerZone>();
            _equipmentBrokerZone.NotNull(nameof(_equipmentBrokerZone));
        }

        public async Task<EquipmentBrokerZone> Get(short id)
        {
            return await _uow.FindOrThrowAsync<EquipmentBrokerZone>(id);
        }

        public async Task<ICollection<EquipmentBrokerZone>> Get()
        {
            return await _equipmentBrokerZone.ToListAsync();
        }
    }
}
