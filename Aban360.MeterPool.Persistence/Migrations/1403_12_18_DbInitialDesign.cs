using Aban360.MeterPool.Persistence.Constants;
using Aban360.MeterPool.Persistence.Extentions;
using Aban360.MeterPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Aban360.MeterPool.Persistence.Migrations
{
    [Migration(14031218)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id);
        int _255 = 255;
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

        private void CreateReadingPeriodType()
        {
            var table = TableName.ReadingPeriodType;
            Create.Table(nameof(TableName.ReadingPeriodType)).InSchema(_schema)
               .WithColumn("Id").AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Days").AsInt16().NotNullable()
               .WithColumn("ClientOrder").AsInt16().NotNullable()//client-zone+1
               .WithColumn("IsEnabled").AsBoolean().NotNullable()
               .WithColumn("HeadquartersId").AsInt16().NotNullable()
               .WithColumn("HeadquartersTitle").AsString(_255).NotNullable();
               
        }

        private void CreateReadingPeriod()
        {
            var table= TableName.ReadingPeriod;
            Create.Table(nameof(TableName.ReadingPeriod)).InSchema(_schema)
                .WithColumn("Id").AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.ReadingPeriodType}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.ReadingPeriodType, table), _schema, nameof(TableName.ReadingPeriodType), Id)
                .WithColumn("ClientOrder").AsInt16().NotNullable();
        }
        private void CreateCounterState()
        {
            var table=TableName.CounterState;
            Create.Table(nameof(TableName.CounterState)).InSchema(_schema)
                .WithColumn("Id").AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("ClientOrder").AsInt16().NotNullable()
                .WithColumn("EnterNumberOption").AsBoolean().NotNullable()
                .WithColumn("NumberRequired").AsBoolean().NotNullable()
                .WithColumn("NonReadable").AsBoolean().NotNullable()
                .WithColumn("NumberLessThanPre").AsBoolean().NotNullable()
                .WithColumn("IsChanged").AsBoolean().NotNullable()
                .WithColumn("IsBroken").AsBoolean().NotNullable()
                .WithColumn("IsNull").AsBoolean().NotNullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                .WithColumn("ImageRequired").AsBoolean().NotNullable()
                .WithColumn("HeadquartersId").AsInt16().NotNullable()
                .WithColumn("HeadquartersTitle").AsString(_255).NotNullable();
        }
        private void CreateReadingConfigDefault()
        {
            var table=TableName.ReadingConfigDefault;
            Create.Table(nameof(TableName.ReadingConfigDefault)).InSchema(_schema)
                .WithColumn("Id").AsInt16().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("NonReadDefault").AsInt16().NotNullable()
                .WithColumn("NonReadMax").AsInt16().NotNullable()
                .WithColumn("NonReadMin").AsInt16().NotNullable()
                .WithColumn("PreNumberDisplayOption").AsBoolean().NotNullable()
                .WithColumn("BillIdDisplayOption").AsBoolean().NotNullable()
                .WithColumn("CustomerNumberDisplayOption").AsBoolean().NotNullable()
                .WithColumn("DomesticLowConstBound").AsInt16().NotNullable()
                .WithColumn("DomesticLowPercentBound").AsInt16().NotNullable()
                .WithColumn("DomesticHighConstBound").AsInt16().NotNullable()
                .WithColumn("DomesticHighPercentBound").AsInt16().NotNullable()
                .WithColumn("ConstructionLowConstBound").AsInt16().NotNullable()
                .WithColumn("ConstructionLowPercentBound").AsInt16().NotNullable()
                .WithColumn("ConstructionHighConstBound").AsInt16().NotNullable()
                .WithColumn("ConstructionHighPercentBound").AsInt16().NotNullable()
                .WithColumn("ContractualCapacityLowConstBound").AsInt16().NotNullable()
                .WithColumn("ContractualCapacityLowPercentBound").AsInt16().NotNullable()
                .WithColumn("ContractualCapacityHighConstBound").AsInt16().NotNullable()
                .WithColumn("ContractualCapacityHighPercentBound").AsInt16().NotNullable()
                .WithColumn("NonDomesticLowPercentRateBound").AsInt16().NotNullable()
                .WithColumn("NonDomesticHighPercentRateBound").AsInt16().NotNullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                .WithColumn("PreDateDisplayOption").AsBoolean().NotNullable()
                .WithColumn("MobileDisplayOption").AsBoolean().NotNullable()
                .WithColumn("DebtDisplayOption").AsBoolean().NotNullable()
                .WithColumn("IconsDisplayOption").AsBoolean().NotNullable()
                .WithColumn("HeadquartersId").AsInt16().NotNullable()
                .WithColumn("HeadquartersTitle").AsString(_255).NotNullable();
        }
    }
}
