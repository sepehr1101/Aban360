using Aban360.CommunicationPool.Persistence.Constants;
using Aban360.CommunicationPool.Persistence.Extensions;
using Aban360.CommunicationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CommunicationPool.Persistence.Migrations
{
    [Migration(1404_03_10)]
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

        private void CreateHubEvent()
        {
            var table = TableName.HubEvent;
            Create.Table(nameof(TableName.HubEvent)).InSchema(_schema)
                .WithColumn(Id).AsInt64().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("ConnectionId").AsString(_255).NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("ConnectDateTime").AsDateTime().NotNullable()
                .WithColumn("DisconnectDateTime").AsDateTime().Nullable();

        }
    }
}
