using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.Contexts.Implementations
{
    public partial class InstallationPoolContext
    {
        public virtual DbSet<EquipmentBroker> EquipmentBrokers { get; set; }
        public virtual DbSet<EquipmentBrokerZone> EquipmentBrokerZones{ get; set; }
        public virtual DbSet<SewageEquipmentBroker> SewageEquipmentBrokers { get; set; }
        public virtual DbSet<SewageEquipmentBrokerZone> SewageEquipmentBrokerZones { get; set; }
    }
}
