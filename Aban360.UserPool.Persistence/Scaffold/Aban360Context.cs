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

    public virtual DbSet<ConstructionType> ConstructionTypes { get; set; }

    public virtual DbSet<CordinalDirection> CordinalDirections { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DeepLog> DeepLogs { get; set; }

    public virtual DbSet<Endpoint> Endpoints { get; set; }

    public virtual DbSet<Estate> Estates { get; set; }

    public virtual DbSet<Flat> Flats { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Headquarter> Headquarters { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<IndividualEstate> IndividualEstates { get; set; }

    public virtual DbSet<IndividualEstateRelationType> IndividualEstateRelationTypes { get; set; }

    public virtual DbSet<IndividualTag> IndividualTags { get; set; }

    public virtual DbSet<IndividualTagDefinition> IndividualTagDefinitions { get; set; }

    public virtual DbSet<IndividualType> IndividualTypes { get; set; }

    public virtual DbSet<InvalidLoginReason> InvalidLoginReasons { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobParameter> JobParameters { get; set; }

    public virtual DbSet<JobQueue> JobQueues { get; set; }

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

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<ReadingBlock> ReadingBlocks { get; set; }

    public virtual DbSet<ReadingBound> ReadingBounds { get; set; }

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
            entity.ToTable("App");

            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Captcha>(entity =>
        {
            entity.ToTable("Captcha");

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
            entity.ToTable("CaptchaDisplayMode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(31);
        });

        modelBuilder.Entity<CaptchaLanguage>(entity =>
        {
            entity.ToTable("CaptchaLanguage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(31);
        });

        modelBuilder.Entity<ConstructionType>(entity =>
        {
            entity.ToTable("ConstructionType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<CordinalDirection>(entity =>
        {
            entity.ToTable("CordinalDirection");

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

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<DeepLog>(entity =>
        {
            entity.ToTable("DeepLog");

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
            entity.ToTable("Endpoint");

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
            entity.ToTable("Estate");

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

        modelBuilder.Entity<Flat>(entity =>
        {
            entity.ToTable("Flat");

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
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Headquarters)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Province_REFERS_Headquarters_ProvinceId");
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.ToTable("Individual");

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
            entity.ToTable("IndividualEstate");

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
            entity.ToTable("IndividualEstateRelationType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<IndividualTag>(entity =>
        {
            entity.ToTable("IndividualTag");

            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Individual).WithMany(p => p.IndividualTags)
                .HasForeignKey(d => d.IndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Individual_REFERS_IndividualTag_IndividualId");

            entity.HasOne(d => d.WaterMeterTagDefinition).WithMany(p => p.IndividualTags)
                .HasForeignKey(d => d.WaterMeterTagDefinitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IndividualTagDefinition_REFERS_IndividualTag_IndividualTagDefinitionId");
        });

        modelBuilder.Entity<IndividualTagDefinition>(entity =>
        {
            entity.ToTable("IndividualTagDefinition");

            entity.Property(e => e.Color).HasMaxLength(15);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<IndividualType>(entity =>
        {
            entity.ToTable("IndividualType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<InvalidLoginReason>(entity =>
        {
            entity.ToTable("InvalidLoginReason");

            entity.Property(e => e.Id).ValueGeneratedNever();
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
            entity.ToTable("LogoutReason");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterDiameter>(entity =>
        {
            entity.ToTable("MeterDiameter");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterMaterial>(entity =>
        {
            entity.ToTable("MeterMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterProducer>(entity =>
        {
            entity.ToTable("MeterProducer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterType>(entity =>
        {
            entity.ToTable("MeterType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<MeterUseType>(entity =>
        {
            entity.ToTable("MeterUseType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.ToTable("Module");

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
            entity.ToTable("Municipality");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Zone).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zone_REFERS_Municipality_ZoneId");
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.ToTable("OperationType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Css).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Province");

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
            entity.ToTable("ReadingBlock");

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
            entity.ToTable("ReadingBound");

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

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Headquarters).WithMany(p => p.Regions)
                .HasForeignKey(d => d.HeadquartersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Headquarters_REFERS_Region_HeadquartersId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
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
            entity.ToTable("Siphon");

            entity.Property(e => e.Id).ValueGeneratedNever();
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
            entity.ToTable("SiphonDiameter");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<SiphonMaterial>(entity =>
        {
            entity.ToTable("SiphonMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<SiphonType>(entity =>
        {
            entity.ToTable("SiphonType");

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
            entity.ToTable("SubModule");

            entity.Property(e => e.ClientRoute).HasMaxLength(255);
            entity.Property(e => e.Style).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Module).WithMany(p => p.SubModules)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Module_REFERS_SubModule_ModuleId");
        });

        modelBuilder.Entity<TokenFailureType>(entity =>
        {
            entity.ToTable("TokenFailureType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Usage>(entity =>
        {
            entity.ToTable("Usage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<UseState>(entity =>
        {
            entity.ToTable("UseState");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.LatestLoginDateTime).HasColumnType("datetime");
            entity.Property(e => e.LockTimespan).HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);
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
            entity.ToTable("UserClaim");

            entity.Property(e => e.ClaimValue).HasMaxLength(1023);
            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserClaim_UserId");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogin");

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
            entity.ToTable("UserRole");

            entity.Property(e => e.RemoveLogInfo).HasMaxLength(255);

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
            entity.ToTable("UserToken");

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
            entity.ToTable("WaterMeter");

            entity.Property(e => e.Id).ValueGeneratedNever();
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

            entity.HasOne(d => d.UseState).WithMany(p => p.WaterMeters)
                .HasForeignKey(d => d.UseStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UseState_REFERS_WaterMeter_UseStateId");
        });

        modelBuilder.Entity<WaterMeterSiphon>(entity =>
        {
            entity.ToTable("WaterMeterSiphon");

            entity.Property(e => e.Id).ValueGeneratedNever();

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
            entity.ToTable("WaterMeterTag");

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
            entity.ToTable("WaterMeterTagDefinition");

            entity.Property(e => e.Color).HasMaxLength(15);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("Zone");

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
