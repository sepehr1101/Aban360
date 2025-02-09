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
        int _31 = 31, _255 = 255, _1023 = 1023;
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
                .WithColumn("ProvienceId").AsInt16().NotNullable();
        }
    }
}