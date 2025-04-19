using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Features.Commands.Implementations
{
    internal sealed class EquipmentBrokerZoneCommandService : IEquipmentBrokerZoneCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBrokerZone> _equipmentBrokerZone;
        public EquipmentBrokerZoneCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _equipmentBrokerZone = _uow.Set<EquipmentBrokerZone>();
            _equipmentBrokerZone.NotNull(nameof(_equipmentBrokerZone));
        }

        public async Task Add(EquipmentBrokerZone equipmentBrokerZone)
        {
            await _equipmentBrokerZone.AddAsync(equipmentBrokerZone);
        }

        public async Task Remove(EquipmentBrokerZone equipmentBrokerZone)
        {
            _equipmentBrokerZone.Remove(equipmentBrokerZone);
        }
    }
}
