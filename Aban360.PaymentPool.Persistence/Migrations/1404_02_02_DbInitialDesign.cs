using Aban360.PaymentPool.Persistence.Constants;
using Aban360.PaymentPool.Persistence.Extensions;
using Aban360.PaymentPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.PaymentPool.Persistence.Migrations
{
    [Migration(14040202)]
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

        private void CreateBank()
        {
            var table = TableName.Bank;
            Create.Table(nameof(TableName.Bank)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().Identity().PrimaryKey(NamingHelpers.Pk(table))
                .WithColumn("BankName").AsString(_255).NotNullable()
                .WithColumn("Icon").AsString(int.MaxValue).Nullable()
                .WithColumn("CentralBankCode").AsString(_255).NotNullable()//Todo
                .WithColumn("Description").AsString(int.MaxValue).Nullable();
        }
        private void CreatePaymentProcedure()
        {
            var table = TableName.PaymentProcedure;
            Create.Table(nameof(TableName.PaymentProcedure)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Icon").AsString(int.MaxValue).Nullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable();
        }


    }
}
