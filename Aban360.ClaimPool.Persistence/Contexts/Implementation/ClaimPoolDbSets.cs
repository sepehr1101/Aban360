using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering;
using Aban360.ClaimPool.Domain.Features.People;
using Aban360.ClaimPool.Domain.Features.Registration;
using Aban360.ClaimPool.Domain.Features.WasteWater;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public partial class ClaimPoolContext
    {
        public virtual DbSet<ConstructionType> ConstructionTypes { get; set; }
        public virtual DbSet<Estate> Estates { get; set; }
        public virtual DbSet<Flat> Flats { get; set; }
        public virtual DbSet<Usage> Usages { get; set; }
        public virtual DbSet<MeterDiameter> MeterDiameters { get; set; }
        public virtual DbSet<MeterMaterial> MeterMaterials { get; set; }
        public virtual DbSet<MeterProducer> MeterProducers { get; set; }
        public virtual DbSet<MeterType> MeterTypes { get; set; }
        public virtual DbSet<MeterUseType> MeterUseTypes { get; set; }
        public virtual DbSet<WaterMeter> WaterMeters { get; set; }
        public virtual DbSet<Individual> Individuals { get; set; }
        public virtual DbSet<IndividualEstate> IndividualEstates { get; set; }
        public virtual DbSet<IndividualEstateRelationType> IndividualEstateRelationTypes { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<UseState> UseStates { get; set; }
        public virtual DbSet<Siphon> Siphons { get; set; }
        public virtual DbSet<SiphonDiameter> SiphonDiameters { get; set; }
        public virtual DbSet<SiphonMaterial> SiphonMaterials { get; set; }
        public virtual DbSet<SiphonType> SiphonTypes { get; set; }
    }
}
