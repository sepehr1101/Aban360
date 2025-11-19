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

        private void CreateMeterReadingFile()
        {
            var table = TableName.MeterReadingFile;
            Create.Table(nameof(TableName.MeterReadingFile)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("FileName").AsString(_255).NotNullable()
                .WithColumn("RecordCount").AsInt32().NotNullable()
                .WithColumn("AgentCode").AsInt16().NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("InsertByUserId").AsGuid().NotNullable()
                .WithColumn("InsertDateTime").AsDateTime().NotNullable();
        }
        private void CreateMeterReadingDetial()
        {
            var table = TableName.MeterReadingDetail;
            Create.Table(nameof(TableName.MeterReadingDetail)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("MeterReadingFileId").AsInt32().NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("ZoneTitle").AsString(_255).NotNullable()
                .WithColumn("CustomerNumber").AsInt32().NotNullable()
                .WithColumn("ReadingNumber").AsString(_50).NotNullable()
                .WithColumn("BillId").AsString(_50).NotNullable()
                .WithColumn("AgentCode").AsInt16().NotNullable()
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
                .WithColumn("VillageId").AsInt16().Nullable()
                .WithColumn("IsSpecial").AsBoolean().NotNullable()
                .WithColumn("MeterDiameterId").AsInt16().NotNullable()
                .WithColumn("VirtualCategoryId").AsInt32().NotNullable()

                .WithColumn("TavizDateJalali").AsString(_10).Nullable()
                .WithColumn("TavizCause").AsString(_255).Nullable()
                .WithColumn("TavizRegisterDateJalali").AsString(_10).Nullable()
                .WithColumn("TavizNumber").AsInt32().Nullable()

                .WithColumn("LastMeterDateJalali").AsString(_10).Nullable()
                .WithColumn("LastMeterNumber").AsInt32().Nullable()
                .WithColumn("ConsumpionAverage").AsFloat().Nullable()
                .WithColumn("DebtAmount").AsInt64().Nullable()
                .WithColumn("LastCounterStateCode").AsInt32().Nullable();
        }
        private void CreateFlow()
        {
            var table = TableName.Flow;
            Create.Table(nameof(TableName.Flow)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.MeterReadingDetail}Id").AsInt32().NotNullable()
                .WithColumn($"{TableName.FlowStep}Id").AsInt16().NotNullable()
                .WithColumn("InsertDateTime").AsDateTime().NotNullable()
                .WithColumn("InsertByUserId").AsGuid().NotNullable()
                .WithColumn("RemovedDateTime").AsDateTime().Nullable()
                .WithColumn("RemovedByUserId").AsGuid().Nullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
        private void CreateFlowStep()
        {
            var table =TableName.FlowStep;
            Create.Table(nameof(TableName.FlowStep)).InSchema(_schema)
                .WithColumn(Id).AsInt16().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
    }
}
