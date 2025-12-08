using Aban360.TaxPool.Persistence.Constants;
using Aban360.TaxPool.Persistence.Extensions;
using Aban360.TaxPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.TaxPool.Persistence.Migrations
{
    [Migration(1404080701)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id);
        int _255 = 255, _1023 = 1023;
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

        private void CreateMaaherInvoiceStatus()
        {
            var table = TableName.MaaherInvoiceStatus;
            Create.Table($"{nameof(TableName.MaaherInvoiceStatus)}").InSchema(_schema)
                .WithColumn(Id).AsInt16().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateMaaheerErrors()
        {
            var table = TableName.MaaherErrors;
            Create.Table($"{nameof(TableName.MaaherErrors)}").InSchema( _schema)
                .WithColumn(Id).AsInt16().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("ErrorCode").AsInt32().NotNullable()
                .WithColumn("HttpStatus").AsInt16().NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
    }
}
