using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Aban360Context : DbContext
{
    public Aban360Context()
    {
    }

    public Aban360Context(DbContextOptions<Aban360Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }

    public virtual DbSet<App> Apps { get; set; }

    public virtual DbSet<Captcha> Captchas { get; set; }

    public virtual DbSet<CaptchaDisplayMode> CaptchaDisplayModes { get; set; }

    public virtual DbSet<CaptchaLanguage> CaptchaLanguages { get; set; }

    public virtual DbSet<CompanyService> CompanyServices { get; set; }

    public virtual DbSet<CompanyServiceOffering> CompanyServiceOfferings { get; set; }

    public virtual DbSet<CompanyServiceType> CompanyServiceTypes { get; set; }

    public virtual DbSet<ConstructionType> ConstructionTypes { get; set; }

    public virtual DbSet<CordinalDirection> CordinalDirections { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<CounterState> CounterStates { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DeepLog> DeepLogs { get; set; }

    public virtual DbSet<Endpoint> Endpoints { get; set; }

    public virtual DbSet<Estate> Estates { get; set; }

    public virtual DbSet<EstateBoundType> EstateBoundTypes { get; set; }

    public virtual DbSet<Flat> Flats { get; set; }

    public virtual DbSet<Geteway> Geteways { get; set; }

    public virtual DbSet<Guild> Guilds { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Headquarter> Headquarters { get; set; }

    public virtual DbSet<ImpactSign> ImpactSigns { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<IndividualEstate> IndividualEstates { get; set; }

    public virtual DbSet<IndividualEstateRelationType> IndividualEstateRelationTypes { get; set; }

    public virtual DbSet<IndividualTag> IndividualTags { get; set; }

    public virtual DbSet<IndividualTagDefinition> IndividualTagDefinitions { get; set; }

    public virtual DbSet<IndividualType> IndividualTypes { get; set; }

    public virtual DbSet<InvalidLoginReason> InvalidLoginReasons { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceInstallment> InvoiceInstallments { get; set; }

    public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

    public virtual DbSet<InvoiceLineItemInsertMode> InvoiceLineItemInsertModes { get; set; }

    public virtual DbSet<InvoiceStatus> InvoiceStatuses { get; set; }

    public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobParameter> JobParameters { get; set; }

    public virtual DbSet<JobQueue> JobQueues { get; set; }

    public virtual DbSet<LineItemType> LineItemTypes { get; set; }

    public virtual DbSet<LineItemTypeGroup> LineItemTypeGroups { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LogoutReason> LogoutReasons { get; set; }

    public virtual DbSet<MeterDiameter> MeterDiameters { get; set; }

    public virtual DbSet<MeterMaterial> MeterMaterials { get; set; }

    public virtual DbSet<MeterProducer> MeterProducers { get; set; }

    public virtual DbSet<MeterType> MeterTypes { get; set; }

    public virtual DbSet<MeterUseType> MeterUseTypes { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<Offering> Offerings { get; set; }

    public virtual DbSet<OfferingGroup> OfferingGroups { get; set; }

    public virtual DbSet<OfferingUnit> OfferingUnits { get; set; }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<ReadingBlock> ReadingBlocks { get; set; }

    public virtual DbSet<ReadingBound> ReadingBounds { get; set; }

    public virtual DbSet<ReadingConfigDefault> ReadingConfigDefaults { get; set; }

    public virtual DbSet<ReadingPeriod> ReadingPeriods { get; set; }

    public virtual DbSet<ReadingPeriodType> ReadingPeriodTypes { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Siphon> Siphons { get; set; }

    public virtual DbSet<SiphonDiameter> SiphonDiameters { get; set; }

    public virtual DbSet<SiphonMaterial> SiphonMaterials { get; set; }

    public virtual DbSet<SiphonType> SiphonTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<SubModule> SubModules { get; set; }

    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<TariffCalculationMode> TariffCalculationModes { get; set; }

    public virtual DbSet<TariffConstant> TariffConstants { get; set; }

    public virtual DbSet<TokenFailureType> TokenFailureTypes { get; set; }

    public virtual DbSet<Usage> Usages { get; set; }

    public virtual DbSet<UseState> UseStates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    public virtual DbSet<WaterMeter> WaterMeters { get; set; }

    public virtual DbSet<WaterMeterSiphon> WaterMeterSiphons { get; set; }

    public virtual DbSet<WaterMeterTag> WaterMeterTags { get; set; }

    public virtual DbSet<WaterMeterTagDefinition> WaterMeterTagDefinitions { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Encrypt=False;Database=Aban360;Integrated Security=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AggregatedCounter>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

            entity.ToTable("AggregatedCounter", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<App>(entity =>
        {
            entity.ToTable("App", "UserPool");

            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Captcha>(entity =>
        {
            entity.ToTable("Captcha", "UserPool");

            entity.Property(e => e.BackColor)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Direction)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EncryptionKey).HasMaxLength(1023);
            entity.Property(e => e.FontName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ForeColor)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Noise)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NonceKey).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CaptchaDisplayMode).WithMany(p => p.Captchas)
                .HasForeignKey(d => d.CaptchaDisplayModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaptchaDisplayMode_REFERS_Captcha_CaptchaDisplayModeId");

            entity.HasOne(d => d.CaptchaLanguage).WithMany(p => p.Captchas)
                .HasForeignKey(d => d.CaptchaLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaptchaLanguage_REFERS_Captcha_CaptchaLanguageId");
        });

        modelBuilder.Entity<CaptchaDisplayMode>(entity =>
        {
            entity.ToTable("CaptchaDisplayMode", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(31);
        });

        modelBuilder.Entity<CaptchaLanguage>(entity =>
        {
            entity.ToTable("CaptchaLanguage", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(31);
        });

        modelBuilder.Entity<CompanyService>(entity =>
        {
            entity.ToTable("CompanyService", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CompanyServiceType).WithMany(p => p.CompanyServices)
                .HasForeignKey(d => d.CompanyServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyServiceType_REFERS_CompanyService_CompanyServiceTypeId");
        });

        modelBuilder.Entity<CompanyServiceOffering>(entity =>
        {
            entity.ToTable("CompanyServiceOffering", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CompanyService).WithMany(p => p.CompanyServiceOfferings)
                .HasForeignKey(d => d.CompanyServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyService_REFERS_CompanyServiceOffering_CompanyServiceId");

            entity.HasOne(d => d.Offering).WithMany(p => p.CompanyServiceOfferings)
                .HasForeignKey(d => d.OfferingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_REFERS_CompanyServiceOffering_OfferingId");
        });

        modelBuilder.Entity<CompanyServiceType>(entity =>
        {
            entity.ToTable("CompanyServiceType", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.TariffCalculationMode).WithMany(p => p.CompanyServiceTypes)
                .HasForeignKey(d => d.TariffCalculationModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TariffCalculationMode_REFERS_CompanyServiceType_TariffCalculationModeId");
        });

        modelBuilder.Entity<ConstructionType>(entity =>
        {
            entity.ToTable("ConstructionType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<CordinalDirection>(entity =>
        {
            entity.ToTable("CordinalDirection", "LocationPool");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

            entity.ToTable("Counter", "HangFire");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CounterState>(entity =>
        {
            entity.ToTable("CounterState", "MeterPool");

            entity.Property(e => e.HeadquartersTitle).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", "LocationPool");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<DeepLog>(entity =>
        {
            entity.ToTable("DeepLog", "UserPool");

            entity.Property(e => e.InsertDateTime).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryDb).HasMaxLength(255);
            entity.Property(e => e.PrimaryId).HasMaxLength(63);
            entity.Property(e => e.PrimaryTable).HasMaxLength(255);

            entity.HasOne(d => d.OperationType).WithMany(p => p.DeepLogs)
                .HasForeignKey(d => d.OperationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationType_REFERS_DeepLog_OperationTypeId");
        });

        modelBuilder.Entity<Endpoint>(entity =>
        {
            entity.ToTable("Endpoint", "UserPool");

            entity.Property(e => e.AuthValue).HasMaxLength(255);
            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.SubModule).WithMany(p => p.Endpoints)
                .HasForeignKey(d => d.SubModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubModule_REFERS_Endpoint_SubModuleId");
        });

        modelBuilder.Entity<Estate>(entity =>
        {
            entity.ToTable("Estate", "ClaimPool");

            entity.Property(e => e.Address).HasMaxLength(1023);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
            entity.Property(e => e.X).HasMaxLength(31);
            entity.Property(e => e.Y).HasMaxLength(31);

            entity.HasOne(d => d.ConstructionType).WithMany(p => p.Estates)
                .HasForeignKey(d => d.ConstructionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConstructionType_REFERS_Estate_ConstructionTypeId");

            entity.HasOne(d => d.EstateBoundType).WithMany(p => p.Estates)
                .HasForeignKey(d => d.EstateBoundTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstateBoundType_REFERS_Estate_EstateBoundTypeId");

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_Estate_REFERS_Estate_PreviousId");

            entity.HasOne(d => d.UsageConsumtion).WithMany(p => p.EstateUsageConsumtions)
                .HasForeignKey(d => d.UsageConsumtionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usage_REFERS_Estate_UsageId_Consumption");

            entity.HasOne(d => d.UsageSell).WithMany(p => p.EstateUsageSells)
                .HasForeignKey(d => d.UsageSellId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usage_REFERS_Estate_UsageId_Sell");
        });

        modelBuilder.Entity<EstateBoundType>(entity =>
        {
            entity.ToTable("EstateBoundType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Flat>(entity =>
        {
            entity.ToTable("Flat", "ClaimPool");

            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Estate).WithMany(p => p.Flats)
                .HasForeignKey(d => d.EstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estate_REFERS_Flat_EstateId");
        });

        modelBuilder.Entity<Geteway>(entity =>
        {
            entity.ToTable("Geteway", "ClaimPool");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Guild>(entity =>
        {
            entity.ToTable("Guild", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Usage).WithMany(p => p.Guilds)
                .HasForeignKey(d => d.UsageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usage_REFERS_Guild_UsageId");
        });

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

            entity.ToTable("Hash", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Field).HasMaxLength(100);
        });

        modelBuilder.Entity<Headquarter>(entity =>
        {
            entity.ToTable("Headquarters", "LocationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Headquarters)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Province_REFERS_Headquarters_ProvinceId");
        });

        modelBuilder.Entity<ImpactSign>(entity =>
        {
            entity.ToTable("ImpactSign", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.ToTable("Individual", "ClaimPool");

            entity.Property(e => e.FatherName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.MobileNumbers).HasMaxLength(1023);
            entity.Property(e => e.NationalId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhoneNumbers).HasMaxLength(1023);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);

            entity.HasOne(d => d.IndividualType).WithMany(p => p.Individuals)
                .HasForeignKey(d => d.IndividualTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IndividualType_REFERS_Individual_IndividualTypeId");

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_Individual_REFERS_Individual_PreviousId");
        });

        modelBuilder.Entity<IndividualEstate>(entity =>
        {
            entity.ToTable("IndividualEstate", "ClaimPool");

            entity.HasOne(d => d.Estate).WithMany(p => p.IndividualEstates)
                .HasForeignKey(d => d.EstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estate_REFERS_IndividualEstate_EstateId");

            entity.HasOne(d => d.IndividualEstateRelationType).WithMany(p => p.IndividualEstates)
                .HasForeignKey(d => d.IndividualEstateRelationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IndividualEstateRelationType_REFERS_IndividualEstate_IndividualEstateRelationTypeId");

            entity.HasOne(d => d.Individual).WithMany(p => p.IndividualEstates)
                .HasForeignKey(d => d.IndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Individual_REFERS_IndividualEstate_IndividualId");
        });

        modelBuilder.Entity<IndividualEstateRelationType>(entity =>
        {
            entity.ToTable("IndividualEstateRelationType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<IndividualTag>(entity =>
        {
            entity.ToTable("IndividualTag", "ClaimPool");

            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Individual).WithMany(p => p.IndividualTags)
                .HasForeignKey(d => d.IndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Individual_REFERS_IndividualTag_IndividualId");

            entity.HasOne(d => d.IndividualTagDefinition).WithMany(p => p.IndividualTags)
                .HasForeignKey(d => d.IndividualTagDefinitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IndividualTagDefinition_REFERS_IndividualTag_IndividualTagDefinitionId");
        });

        modelBuilder.Entity<IndividualTagDefinition>(entity =>
        {
            entity.ToTable("IndividualTagDefinition", "ClaimPool");

            entity.Property(e => e.Color).HasMaxLength(15);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<IndividualType>(entity =>
        {
            entity.ToTable("IndividualType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<InvalidLoginReason>(entity =>
        {
            entity.ToTable("InvalidLoginReason", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.InvoiceStatus).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.InvoiceStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceStatus_REFERS_Invoice_InvoiceStatusId");

            entity.HasOne(d => d.InvoiceType).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.InvoiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceType_REFERS_Invoice_InvoiceTypeId");
        });

        modelBuilder.Entity<InvoiceInstallment>(entity =>
        {
            entity.ToTable("InvoiceInstallment", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BillId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DueDateJalali)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DueDateTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceInstallments)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_REFERS_InvoiceInstallment_InvoiceId");
        });

        modelBuilder.Entity<InvoiceLineItem>(entity =>
        {
            entity.ToTable("InvoiceLineItem", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLineItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_REFERS_InvoiceLineItem_InvoiceId");

            entity.HasOne(d => d.InvoiceLineItemInsertMode).WithMany(p => p.InvoiceLineItems)
                .HasForeignKey(d => d.InvoiceLineItemInsertModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceLineItemInsertMode_REFERS_InvoiceLineItem_InvoiceLineItemInsertModeId");

            entity.HasOne(d => d.Offering).WithMany(p => p.InvoiceLineItems)
                .HasForeignKey(d => d.OfferingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_REFERS_InvoiceLineItem_OfferingId");
        });

        modelBuilder.Entity<InvoiceLineItemInsertMode>(entity =>
        {
            entity.ToTable("InvoiceLineItemInsertMode", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<InvoiceStatus>(entity =>
        {
            entity.ToTable("InvoiceStatus", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<InvoiceType>(entity =>
        {
            entity.ToTable("InvoiceType", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

            entity.ToTable("Job", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName").HasFilter("([StateName] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.StateName).HasMaxLength(20);
        });

        modelBuilder.Entity<JobParameter>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Name }).HasName("PK_HangFire_JobParameter");

            entity.ToTable("JobParameter", "HangFire");

            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Job).WithMany(p => p.JobParameters)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_JobParameter_Job");
        });

        modelBuilder.Entity<JobQueue>(entity =>
        {
            entity.HasKey(e => new { e.Queue, e.Id }).HasName("PK_HangFire_JobQueue");

            entity.ToTable("JobQueue", "HangFire");

            entity.Property(e => e.Queue).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FetchedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<LineItemType>(entity =>
        {
            entity.ToTable("LineItemType", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.LineItemTypeGroup).WithMany(p => p.LineItemTypes)
                .HasForeignKey(d => d.LineItemTypeGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItemTypeGroup_REFERS_LineItemType_LineItemTypeGroupId");
        });

        modelBuilder.Entity<LineItemTypeGroup>(entity =>
        {
            entity.ToTable("LineItemTypeGroup", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.ImpactSign).WithMany(p => p.LineItemTypeGroups)
                .HasForeignKey(d => d.ImpactSignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImpactSign_REFERS_LineItemTypeGroup_ImpactSignId");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_List");

            entity.ToTable("List", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<LogoutReason>(entity =>
        {
            entity.ToTable("LogoutReason", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterDiameter>(entity =>
        {
            entity.ToTable("MeterDiameter", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterMaterial>(entity =>
        {
            entity.ToTable("MeterMaterial", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterProducer>(entity =>
        {
            entity.ToTable("MeterProducer", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterType>(entity =>
        {
            entity.ToTable("MeterType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterUseType>(entity =>
        {
            entity.ToTable("MeterUseType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.ToTable("Module", "UserPool");

            entity.Property(e => e.ClientRoute).HasMaxLength(255);
            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.App).WithMany(p => p.Modules)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_App_REFERS_Module_AppId");
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.ToTable("Municipality", "LocationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Zone).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zone_REFERS_Municipality_ZoneId");
        });

        modelBuilder.Entity<Offering>(entity =>
        {
            entity.ToTable("Offering", "CalculationPool");

            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.OfferingGroup).WithMany(p => p.Offerings)
                .HasForeignKey(d => d.OfferingGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferingGroup_REFERS_Offering_OfferingGroupId");

            entity.HasOne(d => d.OfferingUnit).WithMany(p => p.Offerings)
                .HasForeignKey(d => d.OfferingUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferingUnit_REFERS_Offering_OfferingUnitId");
        });

        modelBuilder.Entity<OfferingGroup>(entity =>
        {
            entity.ToTable("OfferingGroup", "CalculationPool");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<OfferingUnit>(entity =>
        {
            entity.ToTable("OfferingUnit", "CalculationPool");

            entity.Property(e => e.Symbol).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.ToTable("OperationType", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Css).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.ToTable("Profession", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Guild).WithMany(p => p.Professions)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guild_REFERS_Profession_GuildId");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Province", "LocationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CordinalDirection).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.CordinalDirectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CordinalDirection_REFERS_Province_CordinalDirectionId");

            entity.HasOne(d => d.Country).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Country_REFERS_Province_CountryId");
        });

        modelBuilder.Entity<ReadingBlock>(entity =>
        {
            entity.ToTable("ReadingBlock", "LocationPool");

            entity.Property(e => e.FromReadingNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ToReadingNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ReadingBound).WithMany(p => p.ReadingBlocks)
                .HasForeignKey(d => d.ReadingBoundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReadingBound_REFERS_ReadingBlock_ReadingBoundId");
        });

        modelBuilder.Entity<ReadingBound>(entity =>
        {
            entity.ToTable("ReadingBound", "LocationPool");

            entity.Property(e => e.FromReadingNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ToReadingNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Zone).WithMany(p => p.ReadingBounds)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zone_REFERS_ReadingBound_ZoneId");
        });

        modelBuilder.Entity<ReadingConfigDefault>(entity =>
        {
            entity.ToTable("ReadingConfigDefault", "MeterPool");

            entity.Property(e => e.HeadquartersTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<ReadingPeriod>(entity =>
        {
            entity.ToTable("ReadingPeriod", "MeterPool");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.ReadingPeriodType).WithMany(p => p.ReadingPeriods)
                .HasForeignKey(d => d.ReadingPeriodTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReadingPeriodType_REFERS_ReadingPeriod_ReadingPeriodTypeId");
        });

        modelBuilder.Entity<ReadingPeriodType>(entity =>
        {
            entity.ToTable("ReadingPeriodType", "MeterPool");

            entity.Property(e => e.HeadquartersTitle).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region", "LocationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Headquarters).WithMany(p => p.Regions)
                .HasForeignKey(d => d.HeadquartersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Headquarters_REFERS_Region_HeadquartersId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "UserPool");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_Role_REFERS_Role_PreviousId");
        });

        modelBuilder.Entity<Schema>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("PK_HangFire_Schema");

            entity.ToTable("Schema", "HangFire");

            entity.Property(e => e.Version).ValueGeneratedNever();
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Server");

            entity.ToTable("Server", "HangFire");

            entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

            entity.Property(e => e.Id).HasMaxLength(200);
            entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Value }).HasName("PK_HangFire_Set");

            entity.ToTable("Set", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Siphon>(entity =>
        {
            entity.ToTable("Siphon", "ClaimPool");

            entity.Property(e => e.InstallationDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstallationLocation).HasMaxLength(255);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_Siphon_REFERS_Siphon_PreviousId");

            entity.HasOne(d => d.SiphonDiameter).WithMany(p => p.Siphons)
                .HasForeignKey(d => d.SiphonDiameterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiphonDiameter_REFERS_Siphon_SiphonDiameterId");

            entity.HasOne(d => d.SiphonMaterial).WithMany(p => p.Siphons)
                .HasForeignKey(d => d.SiphonMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiphonMaterial_REFERS_Siphon_SiphonMaterialId");

            entity.HasOne(d => d.SiphonType).WithMany(p => p.Siphons)
                .HasForeignKey(d => d.SiphonTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiphonType_REFERS_Siphon_SiphonTypeId");
        });

        modelBuilder.Entity<SiphonDiameter>(entity =>
        {
            entity.ToTable("SiphonDiameter", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<SiphonMaterial>(entity =>
        {
            entity.ToTable("SiphonMaterial", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<SiphonType>(entity =>
        {
            entity.ToTable("SiphonType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

            entity.ToTable("State", "HangFire");

            entity.HasIndex(e => e.CreatedAt, "IX_HangFire_State_CreatedAt");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Reason).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.States)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_State_Job");
        });

        modelBuilder.Entity<SubModule>(entity =>
        {
            entity.ToTable("SubModule", "UserPool");

            entity.Property(e => e.ClientRoute).HasMaxLength(255);
            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Module).WithMany(p => p.SubModules)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Module_REFERS_SubModule_ModuleId");
        });

        modelBuilder.Entity<SubscriptionType>(entity =>
        {
            entity.ToTable("SubscriptionType", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.ToTable("Tariff", "CalculationPool");

            entity.Property(e => e.Condition).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FromDateJalali).HasMaxLength(10);
            entity.Property(e => e.ToDateJalali).HasMaxLength(10);

            entity.HasOne(d => d.LineItemType).WithMany(p => p.Tariffs)
                .HasForeignKey(d => d.LineItemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItemType_REFERS_Tariff_LineItemTypeId");

            entity.HasOne(d => d.Offering).WithMany(p => p.Tariffs)
                .HasForeignKey(d => d.OfferingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_REFERS_Tariff_OfferingId");
        });

        modelBuilder.Entity<TariffCalculationMode>(entity =>
        {
            entity.ToTable("TariffCalculationMode", "CalculationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<TariffConstant>(entity =>
        {
            entity.ToTable("TariffConstant", "CalculationPool");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FromDateJalali).HasMaxLength(10);
            entity.Property(e => e.Key).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ToDateJalali).HasMaxLength(10);
        });

        modelBuilder.Entity<TokenFailureType>(entity =>
        {
            entity.ToTable("TokenFailureType", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Usage>(entity =>
        {
            entity.ToTable("Usage", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<UseState>(entity =>
        {
            entity.ToTable("UseState", "ClaimPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "UserPool");

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.LatestLoginDateTime).HasColumnType("datetime");
            entity.Property(e => e.LockTimespan).HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_User_REFERS_User_PreviousId");
        });

        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("UserClaim", "UserPool");

            entity.Property(e => e.ClaimValue).HasMaxLength(1023);

            entity.HasOne(d => d.User).WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserClaim_UserId");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogin", "UserPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AppVersion).HasMaxLength(15);
            entity.Property(e => e.FirstStepDateTime).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.LogInfo).IsUnicode(false);
            entity.Property(e => e.LogoutDateTime).HasColumnType("datetime");
            entity.Property(e => e.TwoStepCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TwoStepExpireDateTime).HasColumnType("datetime");
            entity.Property(e => e.TwoStepInsertDateTime).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
            entity.Property(e => e.WrongPassword).HasMaxLength(1023);

            entity.HasOne(d => d.InvalidLoginReason).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.InvalidLoginReasonId)
                .HasConstraintName("FK_InvalidLoginReason_REFERS_UserLogin_InvalidLoginReasonId");

            entity.HasOne(d => d.LogoutReason).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.LogoutReasonId)
                .HasConstraintName("FK_LogoutReason_REFERS_UserLogin_LogoutReasonId");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_REFERS_UserLogin_UserId");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole", "UserPool");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_REFERS_UserRole_RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserRole_UserId");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserToken", "UserPool");

            entity.Property(e => e.AccessTokenExpiresDateTime).HasColumnType("datetime");
            entity.Property(e => e.AccessTokenHash).HasMaxLength(1023);
            entity.Property(e => e.RefreshTokenExpiresDateTime).HasColumnType("datetime");
            entity.Property(e => e.RefreshTokenIdHash).HasMaxLength(1023);
            entity.Property(e => e.RefreshTokenIdHashSource).HasMaxLength(1023);

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserToken_UserId");
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo");

            entity.HasIndex(e => e.Version, "UC_Version")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        modelBuilder.Entity<WaterMeter>(entity =>
        {
            entity.ToTable("WaterMeter", "ClaimPool");

            entity.Property(e => e.BillId).HasMaxLength(15);
            entity.Property(e => e.BodySerial)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.GuaranteeDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstallationDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InstallationLocation).HasMaxLength(255);
            entity.Property(e => e.ProductDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ReadingNumber)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);

            entity.HasOne(d => d.Estate).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.EstateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estate_REFERS_WaterMeter_EstateId");

            entity.HasOne(d => d.MeterDiameter).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.MeterDiameterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterDiameter_REFERS_WaterMeter_MeterDiameterId");

            entity.HasOne(d => d.MeterMaterial).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.MeterMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterMaterial_REFERS_WaterMeter_MeterMaterialId");

            entity.HasOne(d => d.MeterProducer).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.MeterProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterProducer_REFERS_WaterMeter_MeterProducerId");

            entity.HasOne(d => d.MeterType).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.MeterTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterType_REFERS_WaterMeter_MeterTypeId");

            entity.HasOne(d => d.MeterUseType).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.MeterUseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeterUseType_REFERS_WaterMeter_MeterUseTypeId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_WaterMeter_REFERS_WaterMeter_WaterMeterId");

            entity.HasOne(d => d.Previous).WithMany(p => p.InversePrevious)
                .HasForeignKey(d => d.PreviousId)
                .HasConstraintName("FK_WaterMeter_REFERS_WaterMeter_PreviousId");

            entity.HasOne(d => d.SubscriptionType).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionType_REFERS_WaterMeter_SubscriptionTypeId");

            entity.HasOne(d => d.UseState).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.UseStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UseState_REFERS_WaterMeter_UseStateId");
        });

        modelBuilder.Entity<WaterMeterSiphon>(entity =>
        {
            entity.ToTable("WaterMeterSiphon", "ClaimPool");

            entity.HasOne(d => d.Siphon).WithMany(p => p.WaterMeterSiphons)
                .HasForeignKey(d => d.SiphonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Siphon_REFERS_WaterMeterSiphon_SiphonId");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterSiphons)
                .HasForeignKey(d => d.WaterMeterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaterMeter_REFERS_WaterMeterSiphon_WaterMeterId");
        });

        modelBuilder.Entity<WaterMeterTag>(entity =>
        {
            entity.ToTable("WaterMeterTag", "ClaimPool");

            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterTags)
                .HasForeignKey(d => d.WaterMeterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaterMeter_REFERS_WaterMeterTag_WaterMeterId");

            entity.HasOne(d => d.WaterMeterTagDefinition).WithMany(p => p.WaterMeterTags)
                .HasForeignKey(d => d.WaterMeterTagDefinitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaterMeterTagDefinition_REFERS_WaterMeterTag_WaterMeterTagDefinitionId");
        });

        modelBuilder.Entity<WaterMeterTagDefinition>(entity =>
        {
            entity.ToTable("WaterMeterTagDefinition", "ClaimPool");

            entity.Property(e => e.Color).HasMaxLength(15);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("Zone", "LocationPool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UnstandardCode)
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.HasOne(d => d.Region).WithMany(p => p.Zones)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Region_REFERS_Zone_RegionId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
