using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Queries.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneQueryService : ISewageEquipmentBrokerZoneQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SewageEquipmentBrokerZone> _sewageEquipmentBrokerZone;
        public SewageEquipmentBrokerZoneQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _sewageEquipmentBrokerZone = _uow.Set<SewageEquipmentBrokerZone>();
            _sewageEquipmentBrokerZone.NotNull(nameof(_sewageEquipmentBrokerZone));
        }

        public async Task<SewageEquipmentBrokerZone> Get(short id)
        {
            return await _uow.FindOrThrowAsync<SewageEquipmentBrokerZone>(id);
        }

        public async Task<ICollection<SewageEquipmentBrokerZone>> Get()
        {
            return await _sewageEquipmentBrokerZone.ToListAsync();
        }
    }
}
