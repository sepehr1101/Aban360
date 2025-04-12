using Aban360.ClaimPool.Persistence.Constants;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.ClaimPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.ClaimPool.Persistence.Migrations
{
    [Migration(1403112101)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _15 = 15, _10 = 10;
        public override void Up()
        {
            Create.Schema(_schema);

            var methods =
               GetType()
              .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(m => m.Name.StartsWith("Create"))
              .ToList();
            methods.ForEach(m => m.Invoke(this, null));
        }
        public override void Down()
        {
            var tableNames =
              GetType()
             .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
             .Where(m => m.Name.StartsWith("Create"))
             .Select(m => m.Name.Replace("Create", string.Empty))
             .ToList();
            tableNames.ForEach(t => Delete.Table(t));
        }

        private void CreateUsage()
        {
            var table = TableName.Usage;
            Create.Table(nameof(TableName.Usage)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("ProvinceId").AsInt16().NotNullable();
        }
        private void CreateGuild()
        {
            var table = TableName.Guild;
            Create.Table(nameof(TableName.Guild)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("UsageId").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Usage, table), _schema, nameof(TableName.Usage), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
        private void CreateProfession()
        {
            var table = TableName.Profession;
            Create.Table(nameof(TableName.Profession)).InSchema(_schema)
               .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
               .WithColumn("GuildId").AsInt16().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.Guild, table), _schema, nameof(TableName.Guild), Id)
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_255).Nullable();
        }
        private void CreateConstructionType()
        {
            var table = TableName.ConstructionType;
            Create.Table(nameof(TableName.ConstructionType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateEstateBoundType()
        {
            var table = TableName.EstateBoundType;
            Create.Table(nameof(TableName.EstateBoundType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateEstate(TableName table, string nameOfTable)
        {
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("ConstructionTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.ConstructionType, table), _schema, nameof(TableName.ConstructionType), Id)
                .WithColumn("EstateBoundTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.EstateBoundType, table), _schema, nameof(TableName.EstateBoundType), Id)
                .WithColumn("PostalCode").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("X").AsString(_31).Nullable()
                .WithColumn("Y").AsString(_31).Nullable()
                .WithColumn("Parcel").AsString(int.MaxValue).Nullable()
                .WithColumn("Address").AsString(_1023).NotNullable()
                .WithColumn("MunipulityId").AsInt32().NotNullable()
                .WithColumn("UsageSellId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Usage, table) + "_Sell", _schema, nameof(TableName.Usage), Id)
                .WithColumn("UsageConsumtionId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Usage, table) + "_Consumption", _schema, nameof(TableName.Usage), Id)
                .WithColumn("UnitDomesticWater").AsInt16().NotNullable()
                .WithColumn("UnitCommercialWater").AsInt16().NotNullable()
                .WithColumn("UnitOtherWater").AsInt16().NotNullable()
                .WithColumn("UnitDomesticSewage").AsInt16().NotNullable()
                .WithColumn("UnitCommercialSewage").AsInt16().NotNullable()
                .WithColumn("UnitOtherSewage").AsInt16().NotNullable()
                .WithColumn("EmptyUnit").AsInt16().NotNullable()
                .WithColumn("HouseholdNumber").AsInt16().NotNullable()
                .WithColumn("Premises").AsInt32().NotNullable()
                .WithColumn("ImprovementsOverall").AsInt32().NotNullable()
                .WithColumn("ImprovementsDomestic").AsInt32().NotNullable()
                .WithColumn("ImprovementsCommercial").AsInt32().NotNullable()
                .WithColumn("ImprovementsOther").AsInt32().NotNullable()
                .WithColumn("ContractualCapacity").AsInt32().NotNullable()
                .WithColumn("Storeys").AsInt16().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("PreviousId").AsInt32().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameOfTable, Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void _CreateFlat(TableName table, string nameOfTable, TableName estateTable, string estateName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("EstateId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(estateTable, table), _schema, estateName, Id)
                .WithColumn("PostalCode").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("Storey").AsInt16()
                .WithColumn("Description").AsString(_1023).Nullable();
        }

        private void CreateEstates()
        {
            _CreateEstate(TableName.Estate, nameof(TableName.Estate));
            _CreateEstate(TableName.RequestEstate, nameof(TableName.RequestEstate));
        }
        private void CreateFlats()
        {
            _CreateFlat(TableName.Flat, nameof(TableName.Flat), TableName.Estate, nameof(TableName.Estate));
            _CreateFlat(TableName.RequestFlat, nameof(TableName.RequestFlat), TableName.RequestEstate, nameof(TableName.RequestEstate));

        }
        private void CreateWaterResource()
        {
            var table = TableName.WaterResource;
            Create.Table(nameof(TableName.WaterResource)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("HeadquartersId").AsInt16().NotNullable()
                .WithColumn("HeadquartersTitle").AsString(_255).NotNullable();
        }
        private void CreateEstateWaterResource()
        {
            var table = TableName.EstateWaterResource;
            Create.Table(nameof(TableName.EstateWaterResource)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn($"{TableName.Estate}Id").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Estate, table), _schema, nameof(TableName.Estate), Id)
                .WithColumn($"{TableName.WaterResource}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.WaterResource, table), _schema, nameof(TableName.WaterResource), Id);
        }


        private void CreateMeterDiameter()
        {
            var table = TableName.MeterDiameter;
            Create.Table(nameof(TableName.MeterDiameter)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterType()
        {
            var table = TableName.MeterType;
            Create.Table(nameof(TableName.MeterType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterProducer()
        {
            var table = TableName.MeterProducer;
            Create.Table(nameof(TableName.MeterProducer)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterMaterial()
        {
            var table = TableName.MeterMaterial;
            Create.Table(nameof(TableName.MeterMaterial)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterUseType()
        {
            var table = TableName.MeterUseType;
            Create.Table(nameof(TableName.MeterUseType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateUseState()
        {
            var table = TableName.UseState;
            Create.Table(nameof(TableName.UseState)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSubscriptionType()
        {
            var table = TableName.SubscriptionType;
            Create.Table(nameof(TableName.SubscriptionType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateWaterMeter(TableName table, string nameOfTable, TableName estateTable, string estateName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
              .WithColumn("ReadingNumber").AsAnsiString(_31).Nullable()
              .WithColumn("CustomerNumber").AsInt32().NotNullable()
              .WithColumn("BillId").AsString(15).NotNullable()
              .WithColumn("EstateId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(estateTable, table), _schema, estateName, Id)
              .WithColumn("UseStateId").AsInt16().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.UseState, table), _schema, nameof(TableName.UseState), Id)
              .WithColumn("SubscriptionTypeId").AsInt16().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.SubscriptionType, table), _schema, nameof(TableName.SubscriptionType), Id)
              .WithColumn("InstallationLocation").AsString(_255).Nullable()
              .WithColumn("BodySerial").AsAnsiString(_31).Nullable()
              .WithColumn("InstallationDate").AsAnsiString(10).Nullable()
              .WithColumn("ProductDate").AsAnsiString(10).Nullable()
              .WithColumn("GuaranteeDate").AsAnsiString(10).Nullable()
              .WithColumn("MeterDiameterId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterDiameter, table), _schema, nameof(TableName.MeterDiameter), Id)
              .WithColumn("MeterProducerId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterProducer, table), _schema, nameof(TableName.MeterProducer), Id)
              .WithColumn("MeterTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterType, table), _schema, nameof(TableName.MeterType), Id)
              .WithColumn("MeterMaterialId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterMaterial, table), _schema, nameof(TableName.MeterMaterial), Id)
              .WithColumn("MeterUseTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterUseType, table), _schema, nameof(TableName.MeterUseType), Id)
              .WithColumn("ParentId").AsInt32().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.WaterMeter, table), _schema, nameOfTable, Id)
              .WithColumn("UserId").AsGuid().NotNullable()
              .WithColumn("PreviousId").AsInt32().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameOfTable, Id)
              .WithColumn("ValidFrom").AsDateTime2().NotNullable()
              .WithColumn("ValidTo").AsDateTime2().Nullable()
              .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
              .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
              .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateWaterMeters()
        {
            _CreateWaterMeter(TableName.WaterMeter, nameof(TableName.WaterMeter), TableName.Estate, nameof(TableName.Estate));
            _CreateWaterMeter(TableName.RequestWaterMeter, nameof(TableName.RequestWaterMeter), TableName.RequestEstate, nameof(TableName.RequestEstate));
        }

        private void CreateWaterMeterTagDefinition()
        {
            var table = TableName.WaterMeterTagDefinition;
            Create.Table(nameof(TableName.WaterMeterTagDefinition)).InSchema(_schema)
                .WithColumn(Id).AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Color").AsString(_15).Nullable();
        }

        private void _CreateWaterMeterTag(TableName table,string nameOfTable,TableName waterMeterTable,string waterMeterName)
        {
           // var table = TableName.WaterMeterTag;
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn(Id).AsInt32().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("WaterMeterId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(waterMeterTable, table), _schema, waterMeterName, Id)
                .WithColumn("WaterMeterTagDefinitionId").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.WaterMeterTagDefinition, table), _schema, nameof(TableName.WaterMeterTagDefinition), Id)
                .WithColumn("Value").AsString(_255).Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }

        private void CreateWaterMeterTag()
        {
            _CreateWaterMeterTag(TableName.WaterMeterTag,nameof(TableName.WaterMeterTag),
                TableName.WaterMeter,nameof(TableName.WaterMeter));

            _CreateWaterMeterTag(TableName.RequestWaterMeterTag, nameof(TableName.RequestWaterMeterTag), 
                TableName.RequestWaterMeter, nameof(TableName.RequestWaterMeter));
        }

        private void CreateIndividualType()
        {
            var table = TableName.IndividualType;
            Create.Table(nameof(TableName.IndividualType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateIndividual(TableName table, string nameOfTable, TableName watermeterTable, string watermeterName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn($"WaterMeterId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(watermeterTable, table), _schema, watermeterName, Id)
                .WithColumn("IndividualTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.IndividualType, table), _schema, nameof(TableName.IndividualType), Id)
                .WithColumn("FullName").AsString(_255).NotNullable()
                .WithColumn("NationalId").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("FatherName").AsString(_255).Nullable()
                .WithColumn("PhoneNumbers").AsString(_1023).Nullable()
                .WithColumn("MobileNumbers").AsString(_1023).Nullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("PreviousId").AsInt32().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameOfTable, Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable(); ;
        }
        private void CreateIndividuals()
        {
            _CreateIndividual(TableName.Individual, nameof(TableName.Individual), TableName.WaterMeter, nameof(TableName.WaterMeter));
            _CreateIndividual(TableName.RequestIndividual, nameof(TableName.RequestIndividual), TableName.RequestWaterMeter, nameof(TableName.RequestWaterMeter));
        }
        private void CreateIndividualEstateRelationType()
        {
            var table = TableName.IndividualEstateRelationType;
            Create.Table(nameof(TableName.IndividualEstateRelationType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateIndividualEstate(TableName table, string nameOfTable, TableName individualTable, string individualName, TableName estateTable, string estateName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn($"IndividualId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(individualTable, table), _schema, individualName, Id)
                .WithColumn("EstateId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(estateTable, table), _schema, estateName, Id)
                .WithColumn("IndividualEstateRelationTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.IndividualEstateRelationType, table), _schema, nameof(TableName.IndividualEstateRelationType), Id);
        }
        private void CreateIndividualEstates()
        {
            _CreateIndividualEstate(TableName.IndividualEstate, nameof(TableName.IndividualEstate), TableName.Individual, nameof(TableName.Individual), TableName.Estate, nameof(TableName.Estate));
            _CreateIndividualEstate(TableName.RequestIndividualEstate, nameof(TableName.RequestIndividualEstate), TableName.RequestIndividual, nameof(TableName.RequestIndividual), TableName.RequestEstate, nameof(TableName.RequestIndividual));
        }

        private void CreateIndividualTagDefinition()
        {
            var table = TableName.IndividualTagDefinition;
            Create.Table(nameof(TableName.IndividualTagDefinition)).InSchema(_schema)
                .WithColumn(Id).AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Color").AsString(_15).Nullable();
        }
        private void _CreateIndividualTag(TableName table, string nameOfTable, TableName individualTable, string individualName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
                .WithColumn(Id).AsInt32().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("IndividualId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(individualTable, table), _schema, individualName, Id)
                .WithColumn("IndividualTagDefinitionId").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.IndividualTagDefinition, table), _schema, nameof(TableName.IndividualTagDefinition), Id)
                .WithColumn("Value").AsString(_255).Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }

        private void CreateIndividualTag()
        {
            _CreateIndividualTag(TableName.IndividualTag, nameof(TableName.IndividualTag),
                TableName.Individual, nameof(TableName.Individual));

            _CreateIndividualTag(TableName.RequestIndividualTag, nameof(TableName.RequestIndividualTag),
                TableName.RequestIndividual, nameof(TableName.RequestIndividual));
        }


        private void CreateSiphonDiameter()
        {
            var table = TableName.SiphonDiameter;
            Create.Table(nameof(TableName.SiphonDiameter)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSiphonType()
        {
            var table = TableName.SiphonType;
            Create.Table(nameof(TableName.SiphonType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSiphonMaterial()
        {
            var table = TableName.SiphonMaterial;
            Create.Table(nameof(TableName.SiphonMaterial)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateSiphon(TableName table, string nameOfTable, TableName waterMeterTable, string waterMeterName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("WaterMeterId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(waterMeterTable, table), _schema, waterMeterName, Id)
              .WithColumn("InstallationLocation").AsString(_255).Nullable()
              .WithColumn("InstallationDate").AsAnsiString(10).Nullable()
              .WithColumn("SiphonDiameterId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonDiameter, table), _schema, nameof(TableName.SiphonDiameter), Id)
              .WithColumn("SiphonTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonType, table), _schema, nameof(TableName.SiphonType), Id)
              .WithColumn("SiphonMaterialId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonMaterial, table), _schema, nameof(TableName.SiphonMaterial), Id)
              .WithColumn("UserId").AsGuid().NotNullable()
              .WithColumn("PreviousId").AsInt32().Nullable()
                   .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameOfTable, Id)
              .WithColumn("ValidFrom").AsDateTime2().NotNullable()
              .WithColumn("ValidTo").AsDateTime2().Nullable()
              .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
              .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
              .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void _CreateWaterMeterSiphon(TableName table, string nameOfTable,TableName waterMeterTable,string waterMeterName,TableName siphonTable,string siphonName)
        {
            Create.Table(nameOfTable).InSchema(_schema)
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
              .WithColumn("WaterMeterId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(waterMeterTable, table), _schema, waterMeterName, Id)
              .WithColumn("SiphonId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(siphonTable, table), _schema, siphonName, Id);
        }
        private void CreateSiphons()
        {
            _CreateSiphon(TableName.Siphon, nameof(TableName.Siphon),
                TableName.WaterMeter,nameof(TableName.WaterMeter));
            _CreateSiphon(TableName.RequestSiphon, nameof(TableName.RequestSiphon),
                TableName.RequestWaterMeter, nameof(TableName.RequestWaterMeter));
        }
        private void CreateWaterMeterSiphon()
        {
            _CreateWaterMeterSiphon(TableName.WaterMeterSiphon, nameof(TableName.WaterMeterSiphon),
                TableName.WaterMeter,nameof(TableName.WaterMeter),
                TableName.Siphon,nameof(TableName.Siphon));

            _CreateWaterMeterSiphon(TableName.RequestWaterMeterSiphon, nameof(TableName.RequestWaterMeterSiphon),
                TableName.RequestWaterMeter, nameof(TableName.RequestWaterMeter),
                TableName.RequestSiphon,nameof(TableName.RequestSiphon));
        }

        private void CreateGateway()
        {
            var table = TableName.Gateway;
            Create.Table(nameof(TableName.Gateway)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateChangeMeterReason()
        {
            var table = TableName.ChangeMeterReason;
            Create.Table(nameof(TableName.ChangeMeterReason)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }



        private void CreateUsageLevel1()
        {
            var table = TableName.UsageLevel1;
            Create.Table(nameof(TableName.UsageLevel1)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateUsageLevel2()
        {
            var table = TableName.UsageLevel2;
            Create.Table(nameof(TableName.UsageLevel2)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel1}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel1, table), _schema, nameof(TableName.UsageLevel1), Id);
        }

        private void CreateUsageLevel3()
        {
            var table = TableName.UsageLevel3;
            Create.Table(nameof(TableName.UsageLevel3)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel2}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel2, table), _schema, nameof(TableName.UsageLevel2), Id);
        }

        private void CreateUsageLevel4()
        {
            var table = TableName.UsageLevel4;
            Create.Table(nameof(TableName.UsageLevel4)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel3}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel3, table), _schema, nameof(TableName.UsageLevel3), Id);
        }

        private void CreateUserWorkday()
        {
            var table = TableName.UserWorkday;
            Create.Table(nameof(TableName.UserWorkday)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UserFullname").AsString(_255).NotNullable()
                .WithColumn("FromReadingNumber").AsAnsiString(_31).NotNullable()
                .WithColumn("ToReadingNumber").AsAnsiString(_31).NotNullable()
                .WithColumn("DateJalali").AsString(_10).NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("ZoneTitle").AsString(_255).NotNullable();
        }

        private void CreateUserLeave()
        {
            var table = TableName.UserLeave;
            Create.Table(nameof(TableName.UserLeave)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("RegiatereId").AsInt16().NotNullable()
                .WithColumn("RegiatereFullname").AsString(_255).NotNullable()
                .WithColumn("RegiatereDatetime").AsDateTime().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UserFullname").AsString(_255).NotNullable()
                .WithColumn("FromDateJalali").AsString(_10).NotNullable()
                .WithColumn("ToDateJalali").AsString(_10).NotNullable();
        }

        private void CreateOfficialHoliday()
        {
            var table = TableName.OfficialHoliday;
            Create.Table(nameof(TableName.OfficialHoliday)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("DateJalali").AsString(_10).NotNullable();
        }

        private void CreateDocumentEntity()
        {
            var table = TableName.DocumentEntity;
            Create.Table(nameof(TableName.DocumentEntity)).InSchema(_schema)
                .WithColumn("Id").AsInt64().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("DocumentId").AsGuid().NotNullable()
                .WithColumn("TableId").AsInt64().NotNullable()
                .WithColumn("RelationEntityId").AsInt16().NotNullable();
        }

    }
}