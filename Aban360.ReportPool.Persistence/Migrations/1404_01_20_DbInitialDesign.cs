using Aban360.ReportPool.Persistence.Contstants;
using Aban360.ReportPool.Persistence.Extentions;
using Aban360.ReportPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System;
using System.Reflection;

namespace Aban360.ReportPool.Persistence.Migrations
{
    [Migration(14040120)]
    public class DbInitialDesing : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id), Hash = nameof(Hash);
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
        public void CreateDynamicReport()
        {
            var table = TableName.DynamicReport;
            Create.Table(nameof(TableName.DynamicReport)).InSchema(_schema)
                .WithColumn(Id).AsInt32().Identity().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Name").AsString(_255).NotNullable()
                .WithColumn("Version").AsInt64().NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UserName").AsString(_255).NotNullable()
                .WithColumn("DocumentId").AsGuid().NotNullable()
                .WithColumn("ValidFrom").AsDateTime().NotNullable()
                .WithColumn("ValidTo").AsDateTime().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }

    }
}
