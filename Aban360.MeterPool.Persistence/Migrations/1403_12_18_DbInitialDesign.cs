using Aban360.MeterPool.Persistence.Constants;
using Aban360.MeterPool.Persistence.Extentions;
using Aban360.MeterPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

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
               .WithColumn("ClientOrder").AsInt32().NotNullable()
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
                .WithColumn("ClientOrder").AsInt32().NotNullable();
        }
    }
}
