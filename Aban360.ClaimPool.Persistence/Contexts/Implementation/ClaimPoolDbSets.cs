using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Contexts.Implementation
{
    public partial class ClaimPoolContext
    {
        public virtual DbSet<ConstructionType> ConstructionTypes { get; set; }
        public virtual DbSet<EstateBoundType> EstateBoundTypes { get; set; }
        public virtual DbSet<Estate> Estates { get; set; }
        public virtual DbSet<CapacityCalculationIndex> CapacityCalculationIndices{ get; set; }
        public virtual DbSet<Flat> Flats { get; set; }
        public virtual DbSet<EstateWaterResource> EstateWaterResources { get; set; }
        public virtual DbSet<WaterResource> WaterResources { get; set; }
        public virtual DbSet<Usage> Usages { get; set; }
        public virtual DbSet<Guild> Guilds { get; set; }
        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<MeterDiameter> MeterDiameters { get; set; }
        public virtual DbSet<MeterMaterial> MeterMaterials { get; set; }
        public virtual DbSet<MeterProducer> MeterProducers { get; set; }
        public virtual DbSet<MeterType> MeterTypes { get; set; }
        public virtual DbSet<MeterUseType> MeterUseTypes { get; set; }
        public virtual DbSet<WaterMeter> WaterMeters { get; set; }
        public virtual DbSet<IndividualType> IndividualTypes { get; set; }
        public virtual DbSet<Individual> Individuals { get; set; }
        public virtual DbSet<IndividualEstate> IndividualEstates { get; set; }
        public virtual DbSet<IndividualEstateRelationType> IndividualEstateRelationTypes { get; set; }
        public virtual DbSet<DiscountType> DiscountTypes{ get; set; }
        public virtual DbSet<IndividualDiscountType> IndividualDiscountTypes{ get; set; }
        public virtual DbSet<RequestIndividualDiscountType> RequestIndividualDiscountTypes{ get; set; }

        public virtual DbSet<UseState> UseStates { get; set; }
        public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public virtual DbSet<Handover> Handovers{ get; set; }


        public virtual DbSet<Siphon> Siphons { get; set; }
        public virtual DbSet<SiphonDiameter> SiphonDiameters { get; set; }
        public virtual DbSet<SiphonMaterial> SiphonMaterials { get; set; }
        public virtual DbSet<SiphonType> SiphonTypes { get; set; }
        public virtual DbSet<WaterMeterSiphon> WaterMeterSiphons { get; set; }
        public virtual DbSet<WaterMeterTagDefinition> WaterMeterTagDefinitions { get; set; }
        public virtual DbSet<WaterMeterInstallationMethod> WaterMeterInstallationStructures{ get; set; }

        public virtual DbSet<WaterMeterTag> WaterMeterTags { get; set; }
        public virtual DbSet<IndividualTagDefinition> IndividualTagDefinitions { get; set; }
        public virtual DbSet<IndividualTag> IndividualTags { get; set; }
        public virtual DbSet<Gateway> Geteways { get; set; }
        public virtual DbSet<ChangeMeterReason> ChangeMeterReasons { get; set; }

        public virtual DbSet<UsageLevel1> UsageLevels { get; set; }
        public virtual DbSet<UsageLevel2> UsageLevel2s { get; set; }
        public virtual DbSet<UsageLevel3> UsageLevel3s { get; set; }
        public virtual DbSet<UsageLevel4> UsageLevel4s { get; set; }
        public virtual DbSet<UserLeave> UserLeaves { get; set; }
        public virtual DbSet<UserWorkday> UserWorkdays { get; set; }
        public virtual DbSet<OfficialHoliday> OfficialHolidays { get; set; }

        public virtual DbSet<RequestEstate> RequestEstate { get; set; }
        public virtual DbSet<RequestFlat> RequestFlats { get; set; }
        public virtual DbSet<RequestIndividual> RequestIndividuals { get; set; }
        public virtual DbSet<RequestIndividualEstate> RequestIndividualEstates { get; set; }
        public virtual DbSet<RequestIndividualTag> RequestIndividualTags { get; set; }
        public virtual DbSet<RequestSiphon> RequestSiphons { get; set; }
        public virtual DbSet<RequestWaterMeter> RequestWaterMeters { get; set; }
        public virtual DbSet<RequestWaterMeterSiphon> RequestWaterMeterSiphons { get; set; }
        public virtual DbSet<RequestWaterMeterTag> RequestWaterMeterTags { get; set; }
        public virtual DbSet<RequestTracking> RequestTrackings{ get; set; }

    }
}
