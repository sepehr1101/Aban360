using Aban360.InstallationPool.Persistence.Constants;
using Aban360.InstallationPool.Persistence.Extensions;
using Aban360.InstallationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.InstallationPool.Persistence.Migrations
{
    [Migration(14030130)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Title = nameof(Title), Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _10 = 10;
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

        private void CreateEquipmentBroker()
        {
            var table = TableName.EquipmentBroker;
            Create.Table(nameof(TableName.EquipmentBroker)).InSchema(_schema)
                .WithColumn("Id").AsInt16().Identity().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Website").AsString(_1023).NotNullable()
                .WithColumn("ApiUrl").AsString(_1023).NotNullable();
        }
        private void CreateEquipmentBrokerZone()
        {
            var table = TableName.EquipmentBrokerZone;
            Create.Table(nameof(TableName.EquipmentBrokerZone)).InSchema(_schema)
                .WithColumn("Id").AsInt16().Identity().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("ZoneTitle").AsString(_255).NotNullable()
                .WithColumn($"{TableName.EquipmentBroker}Id").AsInt16().NotNullable()
                .ForeignKey(NamingHelper.Fk(TableName.EquipmentBroker, table), _schema, nameof(TableName.EquipmentBroker), Id);
        }
    }
}
