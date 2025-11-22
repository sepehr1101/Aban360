using Aban360.CalculationPool.Persistence.Constants;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.CalculationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CalculationPool.Persistence.Migrations
{
    [Migration(1404082801)]
    public class MeterReadingInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id);
        int _10 = 10, _50 = 50, _255 = 255;
        public override void Up()
        {
            var method =
                GetType()
               .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
               .Where(m => m.Name.StartsWith("Create"))
               .ToList();
            method.ForEach(m => m.Invoke(this, null));
        }
        public override void Down()
        {
            var tableName =
                 GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name.StartsWith("Create"))
                .Select(m => m.Name.Replace("Create", string.Empty))
                .ToList();
            tableName.ForEach(t => Delete.Table(t));
        }

      
        private void _CreateMeterReadingDetial()
        {
            var table = TableName.MeterReadingDetail;
            Create.Table(nameof(TableName.MeterReadingDetail)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("FlowImportedId").AsInt32().NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("CustomerNumber").AsInt32().NotNullable()
                .WithColumn("ReadingNumber").AsString(_50).NotNullable()
                .WithColumn("BillId").AsString(_50).NotNullable()
                .WithColumn("AgentCode").AsInt32().NotNullable()
                .WithColumn("CurrentCounterStateCode").AsInt16().NotNullable()
                .WithColumn("PreviousDateJalali").AsString(_10).NotNullable()
                .WithColumn("CurrentDateJalali").AsString(_10).NotNullable()
                .WithColumn("PreviousNumber").AsInt32().NotNullable()
                .WithColumn("CurrentNumber").AsInt32().NotNullable()

                .WithColumn("ExcludedByUserId").AsGuid().Nullable()
                .WithColumn("ExcludedDateTime").AsDateTime().Nullable()

                .WithColumn("InsertByUserId").AsGuid().NotNullable()
                .WithColumn("InsertDateTime").AsDateTime().NotNullable()
                .WithColumn("RemovedByUserId").AsGuid().Nullable()
                .WithColumn("RemovedDateTime").AsDateTime().Nullable()

                .WithColumn("UsageId").AsInt32().NotNullable()
                .WithColumn("DomesticUnit").AsInt32().NotNullable()
                .WithColumn("CommercialUnit").AsInt32().NotNullable()
                .WithColumn("OtherUnit").AsInt32().NotNullable()
                .WithColumn("EmptyUnit").AsInt32().NotNullable()
                .WithColumn("WaterInstallationDateJalali").AsString(_10).NotNullable()
                .WithColumn("SewageInstallationDateJalali").AsString(_10).Nullable()
                .WithColumn("WaterRegisterDate").AsString(_10).NotNullable()
                .WithColumn("SewageRegisterDate").AsString(_10).Nullable()
                .WithColumn("WaterCount").AsInt32().NotNullable()
                .WithColumn("SewageCalcState").AsInt32().NotNullable()
                .WithColumn("ContractualCapacity").AsInt32().NotNullable()
                .WithColumn("HouseholdNumber").AsInt32().NotNullable()
                .WithColumn("HouseholdDate").AsString(_50).Nullable()
                .WithColumn("VillageId").AsString().Nullable()
                .WithColumn("IsSpecial").AsBoolean().NotNullable()
                .WithColumn("MeterDiameterId").AsInt16().NotNullable()
                .WithColumn("VirtualCategoryId").AsInt32().NotNullable()

                .WithColumn("TavizDateJalali").AsString(_10).Nullable()
                .WithColumn("TavizCause").AsString(_255).Nullable()
                .WithColumn("TavizRegisterDateJalali").AsString(_10).Nullable()
                .WithColumn("TavizNumber").AsInt32().Nullable()

                .WithColumn("LastMeterDateJalali").AsString(_10).Nullable()
                .WithColumn("LastMeterNumber").AsInt32().Nullable()
                .WithColumn("ConsumptionAverage").AsFloat().Nullable()
                .WithColumn("LastCounterStateCode").AsInt32().Nullable();
        }
        private void _CreateMeterFlow()
        {
            var table = TableName.MeterFlow;
            Create.Table(nameof(TableName.MeterFlow)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.MeterFlowStep}Id").AsInt16().NotNullable()
                .WithColumn("FileName").AsString(_255).NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("InsertDateTime").AsDateTime().NotNullable()
                .WithColumn("InsertByUserId").AsGuid().NotNullable()
                .WithColumn("RemovedDateTime").AsDateTime().Nullable()
                .WithColumn("RemovedByUserId").AsGuid().Nullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
        private void _CreateMeterFlowStep()
        {
            var table =TableName.MeterFlowStep;
            Create.Table(nameof(TableName.MeterFlowStep)).InSchema(_schema)
                .WithColumn(Id).AsInt16().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
    }
}
