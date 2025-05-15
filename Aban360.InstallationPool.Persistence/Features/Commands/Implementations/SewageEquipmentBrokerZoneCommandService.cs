using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneCommandService : ISewageEquipmentBrokerZoneCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SewageEquipmentBrokerZone> _sewageEquipmentBrokerZone;
        public SewageEquipmentBrokerZoneCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _sewageEquipmentBrokerZone = _uow.Set<SewageEquipmentBrokerZone>();
            _sewageEquipmentBrokerZone.NotNull(nameof(_sewageEquipmentBrokerZone));
        }

        public async Task Add(SewageEquipmentBrokerZone sewageEquipmentBrokerZone)
        {
            await _sewageEquipmentBrokerZone.AddAsync(sewageEquipmentBrokerZone);
        }

        public async Task Remove(SewageEquipmentBrokerZone sewageEquipmentBrokerZone)
        {
            _sewageEquipmentBrokerZone.Remove(sewageEquipmentBrokerZone);
        }
    }
}
