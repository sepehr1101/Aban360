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
        int _255 = 255, _1023 = 1023,_24=24;
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
                .WithColumn("CentralBankCode").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable();
        }
        private void CreatePaymentMethod()
        {
            var table = TableName.PaymentMethod;
            Create.Table(nameof(TableName.PaymentMethod)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Icon").AsString(int.MaxValue).Nullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable();
        }

        private void CreateCreditor()
        {
            var table = TableName.CreditorType;
            Create.Table(nameof(TableName.CreditorType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateBankFileStructure()
        {
            var table = TableName.BankFileStructure;
            Create.Table(nameof(TableName.BankFileStructure)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table)).Identity()
                .WithColumn("FromIndex").AsInt16().NotNullable()
                .WithColumn("ToIndex").AsInt16().NotNullable()
                .WithColumn("StringLenght").AsInt16().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("IsHeader").AsBoolean().NotNullable();
        }

        private void CreateAccountType()
        {
            var table =TableName.AccountType;
            Create.Table(nameof(TableName.AccountType)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateBankAccount()
        {
            var table = TableName.BankAccount;
            Create.Table(nameof(TableName.BankAccount)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelpers.Pk(table)).Identity()
                .WithColumn($"{TableName.Bank}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelpers.Fk(TableName.Bank, table), _schema, nameof(TableName.Bank), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.AccountType}Id").AsInt16().NotNullable()
                .WithColumn("RegionId").AsInt32().NotNullable()
                .WithColumn("RegionTitle").AsString(_255).NotNullable()
                .WithColumn("IBan").AsString(_24).NotNullable()
                .WithColumn("Number").AsString(_24).NotNullable();//Todo : lenght
        }

    }
}
