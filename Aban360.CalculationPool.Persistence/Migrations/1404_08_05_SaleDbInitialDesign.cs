using Aban360.CalculationPool.Persistence.Constants;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.CalculationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CalculationPool.Persistence.Migrations
{
    [Migration(1404080501)]
    public class SaleDbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id);
        int _5 = 5, _10 = 10;
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
        private void CreateArticle11()
        {
            var table = TableName.Article11;
            Create.Table($"{nameof(TableName.Article11)}").InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("WaterMeterAmount").AsInt64().NotNullable()
                .WithColumn("WaterAmount").AsInt64().NotNullable()
                .WithColumn("SewageMeterAmount").AsInt64().Nullable()
                .WithColumn("SewageAmount").AsInt64().Nullable()
                .WithColumn("IsDomestic").AsBoolean().NotNullable()
                .WithColumn("BlockCode").AsString(_5).Nullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("FromDateJalali").AsString(_10).NotNullable()
                .WithColumn("ToDateJalali").AsString(_10).NotNullable()
                .WithColumn("RegisterDateTime").AsDateTime().NotNullable()
                .WithColumn("RegisterByUserId").AsGuid().NotNullable()
                .WithColumn("RemoveDateTime").AsDateTime().Nullable()
                .WithColumn("RemoveByUserId").AsGuid().Nullable();
        }
        private void CreateInstallationAndEquipment()
        {
            var table = TableName.InstallationAndEquipment;
            Create.Table(($"{nameof(TableName.InstallationAndEquipment)}")).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("IsWater").AsBoolean().NotNullable()
                .WithColumn("MeterDiameterId").AsInt16().NotNullable()
                .WithColumn("InstallationAmount").AsInt64().NotNullable()
                .WithColumn("EquipmentAmount").AsInt64().NotNullable()
                .WithColumn("FromDateJalali").AsString(_10).NotNullable()
                .WithColumn("ToDateJalali").AsString(_10).NotNullable()
                .WithColumn("RegisterDateTime").AsDateTime().NotNullable()
                .WithColumn("RegisterByUserId").AsGuid().NotNullable()
                .WithColumn("RemoveDateTime").AsDateTime().Nullable()
                .WithColumn("RemoveByUserId").AsGuid().Nullable();
        }

    }
}
