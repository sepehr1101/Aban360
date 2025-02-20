using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.ClaimPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.ClaimPool.Persistence.Migrations
{
    [Migration(1403112101)]
    public class DbInitialDesign : Migration
    {
        string Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _15 = 15;
        public override void Up()
        {
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
            Create.Table(nameof(TableName.Usage))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("ProvinceId").AsInt16().NotNullable();
        }
        private void CreateConstructionType()
        {
            var table = TableName.ConstructionType;
            Create.Table(nameof(TableName.ConstructionType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateEstate()
        {
            var table = TableName.Estate;
            Create.Table(nameof(TableName.Estate))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("ConstructionTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.ConstructionType, table), nameof(TableName.ConstructionType), Id)
                .WithColumn("PostalCode").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("X").AsString(_31).Nullable()
                .WithColumn("Y").AsString(_31).Nullable()
                .WithColumn("Parcel").AsString(int.MaxValue).Nullable()
                .WithColumn("Address").AsString(_1023).NotNullable()
                .WithColumn("MunipulityId").AsInt32()
                .WithColumn("UsageSellId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Usage, table) + "_Sell", nameof(TableName.Usage), Id)
                .WithColumn("UsageConsumtionId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Usage, table) + "_Consumption", nameof(TableName.Usage), Id)
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
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), nameof(TableName.Estate), Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateFlat()
        {
            var table = TableName.Flat;
            Create.Table(nameof(TableName.Flat))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("EstateId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Estate, table), nameof(TableName.Estate), Id)
                .WithColumn("PostalCode").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("Storey").AsInt16()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateIndividualType()
        {
            var table = TableName.IndividualType;
            Create.Table(nameof(TableName.IndividualType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateIndividual()
        {
            var table = TableName.Individual;
            Create.Table(nameof(TableName.Individual))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("IndividualTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.IndividualType, table), nameof(TableName.IndividualType), Id)
                .WithColumn("FullName").AsString(_255).NotNullable()
                .WithColumn("NationalId").AsFixedLengthAnsiString(10).Nullable()
                .WithColumn("FatherName").AsString(_255).Nullable()
                .WithColumn("PhoneNumbers").AsString(_1023).Nullable()
                .WithColumn("MobileNumbers").AsString(_1023).Nullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("PreviousId").AsInt32().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), nameof(TableName.Individual), Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable(); ;
        }
        private void CreateIndividualEstateRelationType()
        {
            var table = TableName.IndividualEstateRelationType;
            Create.Table(nameof(TableName.IndividualEstateRelationType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateIndividualEstate()
        {
            var table = TableName.IndividualEstate;
            Create.Table(nameof(TableName.IndividualEstate))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("IndividualId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Individual, table), nameof(TableName.Individual), Id)
                .WithColumn("EstateId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Estate, table), nameof(TableName.Estate), Id)
                .WithColumn("IndividualEstateRelationTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.IndividualEstateRelationType, table), nameof(TableName.IndividualEstateRelationType), Id);
        }

        private void CreateIndividualTagDefinition()
        {
            var table = TableName.IndividualTagDefinition;
            Create.Table(nameof(TableName.IndividualTagDefinition))
                .WithColumn(Id).AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Color").AsString(_15).Nullable();
        }
        private void CreateIndividualTag()
        {
            var table = TableName.IndividualTag;
            Create.Table(nameof(TableName.IndividualTag))
                .WithColumn(Id).AsInt32().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("IndividualId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Individual, table), nameof(TableName.Individual), Id)
                .WithColumn("IndividualTagDefinitionId").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.IndividualTagDefinition, table), nameof(TableName.IndividualTagDefinition), Id)
                .WithColumn("Value").AsString(_255).Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateMeterDiameter()
        {
            var table = TableName.MeterDiameter;
            Create.Table(nameof(TableName.MeterDiameter))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterType()
        {
            var table = TableName.MeterType;
            Create.Table(nameof(TableName.MeterType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterProducer()
        {
            var table = TableName.MeterProducer;
            Create.Table(nameof(TableName.MeterProducer))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterMaterial()
        {
            var table = TableName.MeterMaterial;
            Create.Table(nameof(TableName.MeterMaterial))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateMeterUseType()
        {
            var table = TableName.MeterUseType;
            Create.Table(nameof(TableName.MeterUseType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateUseState()
        {
            var table = TableName.UseState;
            Create.Table(nameof(TableName.UseState))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateWaterMeter()
        {
            var table = TableName.WaterMeter;
            Create.Table(nameof(TableName.WaterMeter))
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
              .WithColumn("ReadingNumber").AsAnsiString(_31).Nullable()
              .WithColumn("CustomerNumber").AsInt32().NotNullable()
              .WithColumn("BillId").AsString(15).NotNullable()
              .WithColumn("EstateId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.Estate, table), nameof(TableName.Estate), Id)
              .WithColumn("UseStateId").AsInt16().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.UseState, table), nameof(TableName.UseState), Id)
              .WithColumn("InstallationLocation").AsString(_255).Nullable()
              .WithColumn("BodySerial").AsAnsiString(_31).Nullable()
              .WithColumn("InstallationDate").AsAnsiString(10).Nullable()
              .WithColumn("ProductDate").AsAnsiString(10).Nullable()
              .WithColumn("GuaranteeDate").AsAnsiString(10).Nullable()
              .WithColumn("MeterDiameterId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterDiameter, table), nameof(TableName.MeterDiameter), Id)
              .WithColumn("MeterProducerId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterProducer, table), nameof(TableName.MeterProducer), Id)
              .WithColumn("MeterTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterType, table), nameof(TableName.MeterType), Id)
              .WithColumn("MeterMaterialId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterMaterial, table), nameof(TableName.MeterMaterial), Id)
              .WithColumn("MeterUseTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.MeterUseType, table), nameof(TableName.MeterUseType), Id)
              .WithColumn("ParentId").AsInt32().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.WaterMeter, table), nameof(TableName.WaterMeter), Id)
              .WithColumn("UserId").AsGuid().NotNullable()
              .WithColumn("PreviousId").AsInt32().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), nameof(TableName.WaterMeter), Id)
              .WithColumn("ValidFrom").AsDateTime2().NotNullable()
              .WithColumn("ValidTo").AsDateTime2().Nullable()
              .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
              .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
              .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateWaterMeterTagDefinition()
        { 
            var table = TableName.WaterMeterTagDefinition;
            Create.Table(nameof(TableName.WaterMeterTagDefinition))
                .WithColumn(Id).AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Color").AsString(_15).Nullable();
        }

        private void CreateWaterMeterTag()
        {
            var table = TableName.WaterMeterTag;
            Create.Table(nameof(TableName.WaterMeterTag))
                .WithColumn(Id).AsInt32().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("WaterMeterId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.WaterMeter, table), nameof(TableName.WaterMeter), Id)
                .WithColumn("WaterMeterTagDefinitionId").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.WaterMeterTagDefinition, table), nameof(TableName.WaterMeterTagDefinition), Id)
                .WithColumn("Value").AsString(_255).Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateSiphonDiameter()
        {
            var table = TableName.SiphonDiameter;
            Create.Table(nameof(TableName.SiphonDiameter))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSiphonType()
        {
            var table = TableName.SiphonType;
            Create.Table(nameof(TableName.SiphonType))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSiphonMaterial()
        {
            var table = TableName.SiphonMaterial;
            Create.Table(nameof(TableName.SiphonMaterial))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateSiphon()
        {
            var table = TableName.Siphon;
            Create.Table(nameof(TableName.Siphon))
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
              .WithColumn("InstallationLocation").AsString(_255).Nullable()
              .WithColumn("InstallationDate").AsAnsiString(10).Nullable()
              .WithColumn("SiphonDiameterId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonDiameter, table), nameof(TableName.SiphonDiameter), Id)
              .WithColumn("SiphonTypeId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonType, table), nameof(TableName.SiphonType), Id)
              .WithColumn("SiphonMaterialId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SiphonMaterial, table), nameof(TableName.SiphonMaterial), Id)
              .WithColumn("UserId").AsGuid().NotNullable()
              .WithColumn("PreviousId").AsInt32().Nullable()
                   .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), nameof(TableName.Siphon), Id)
              .WithColumn("ValidFrom").AsDateTime2().NotNullable()
              .WithColumn("ValidTo").AsDateTime2().Nullable()
              .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
              .WithColumn("RemoveLogInfo").AsString(int.MinValue).Nullable()
              .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateWaterMeterSiphon()
        {
            var table = TableName.WaterMeterSiphon;
            Create.Table(nameof(TableName.WaterMeterSiphon))
              .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
              .WithColumn("WaterMeterId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.WaterMeter, table), nameof(TableName.WaterMeter), Id)
              .WithColumn("SiphonId").AsInt32().NotNullable()
                  .ForeignKey(NamingHelper.Fk(TableName.Siphon, table), nameof(TableName.Siphon), Id);
        }
    }
}