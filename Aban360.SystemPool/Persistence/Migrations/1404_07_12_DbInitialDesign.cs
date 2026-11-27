using Aban360.SystemPool.Persistence.Extensions;
using Aban360.SystemPool.Persistence.Migrations.Constants;
using Aban360.SystemPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.SystemPool.Persistence.Migrations
{
    [Migration(14040712)]
    public class _1404_07_12_DbInitialDesign:Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id), Hash = nameof(Hash);
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

        private void CreateFaq()
        {
            var table = TableName.Faq;
            Create.Table(nameof(TableName.Faq)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Header").AsString(200).NotNullable()
                .WithColumn("Icon").AsString(100).Nullable()
                .WithColumn("Content").AsString(int.MaxValue).NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("CreatedBy").AsString(100).NotNullable()
                .WithColumn("DeleteDateTime").AsDateTime2().Nullable()
                .WithColumn("DeletedBy").AsString(100).Nullable();
        }
    }
}
