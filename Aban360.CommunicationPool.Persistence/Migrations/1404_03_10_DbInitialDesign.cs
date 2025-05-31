using Aban360.CommunicationPool.Persistence.Constants;
using Aban360.CommunicationPool.Persistence.Extensions;
using Aban360.CommunicationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CommunicationPool.Persistence.Migrations
{
    [Migration(1404_03_10)]
    public class DbInitialDesign:Migration
    {
        string _schema = TableSchema.Name;
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
    }
}
