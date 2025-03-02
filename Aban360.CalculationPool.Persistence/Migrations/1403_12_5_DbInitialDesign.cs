using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.CalculationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Aban360.CalculationPool.Persistence.Migrations
{
    public class DbInitialDesign : Migration
    {
        string Id = nameof(Id);
        int _255 = 255;
        public override void Down()
        {
            var methods =
               GetType()
              .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(m => m.Name.StartsWith("Create"))
              .ToList();
            methods.ForEach(m => m.Invoke(this, null));
        }

        public override void Up()
        {
            var tableNames =
              GetType()
             .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
             .Where(m => m.Name.StartsWith("Create"))
             .Select(m => m.Name.Replace("Create", string.Empty))
             .ToList();
            tableNames.ForEach(t => Delete.Table(t));
        }

        private void CreateOffering()
        {
            var table = TableName.Offering;
            Create.Table(nameof(TableName.Offering))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.OfferingUnit}Id").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingUnit, table), nameof(TableName.OfferingUnit), Id)
                .WithColumn($"{TableName.OfferingGroup}Id").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingGroup, table), nameof(TableName.OfferingGroup), Id)
                .WithColumn("Description").AsString().Nullable();
        }

        private void CreateOfferingUnit()
        {
            var table = TableName.OfferingUnit;
            Create.Table(nameof(TableName.OfferingUnit))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Symbol").AsString(_255).Nullable();
        }

        private void CreateOfferingGroup()
        {
            var table = TableName.OfferingGroup;
            Create.Table(nameof(TableName.OfferingGroup))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateInvoiceType()
        {
            var table = TableName.InvoiceType;
            Create.Table(nameof(TableName.InvoiceType))
                .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }

        private void CreateInvoice()
        {
            var table = TableName.Invoice;
            Create.Table(nameof(TableName.Invoice))
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.InvoiceType}Id").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.InvoiceType, table), nameof(TableName.InvoiceType), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("BillId").AsString(_255).NotNullable()
                .WithColumn("PayId").AsString(_255).NotNullable()
                .WithColumn("DepositId").AsString(_255).NotNullable()
                .WithColumn("OfferingCount").AsInt32().NotNullable()
                .WithColumn("Duration").AsInt32().Nullable()
                .WithColumn("Usage").AsInt32().Nullable()
                .WithColumn("Rate").AsDouble().Nullable()
                .WithColumn("InsertLogId").AsGuid().Nullable()
                .WithColumn("DeleteLogId").AsGuid().Nullable();
        }
        private void CreateInvoiceItem()
        {
            var table = TableName.InvoiceItem;
            Create.Table(nameof(TableName.InvoiceItem))
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.Invoice}Id").AsInt64().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Invoice, table), nameof(TableName.Invoice), Id)
                .WithColumn($"{TableName.Offering}Id").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Offering, table), nameof(TableName.Offering), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("Quanity").AsInt32().NotNullable();
        }
        private void CreateInvoiceInstallment()
        {
            var table = TableName.InvoiceInstallment;
            Create.Table(nameof(TableName.InvoiceInstallment))
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.Invoice}Id").AsInt64().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Invoice, table), nameof(TableName.Invoice), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("Quanity").AsInt32().NotNullable()
                .WithColumn("DueDateJalali").AsDateTime2().NotNullable()//?
                .WithColumn("DueDateTime").AsDateTime().NotNullable()//?
                .WithColumn("InstallmentOrder").AsInt32().NotNullable()//?
                .WithColumn("BillId").AsString().NotNullable()
                .WithColumn("PayId").AsString().NotNullable();
        }

        private void CreateInsertMode()
        {
            var table = TableName.InsertMode;
            Create.Table(nameof(TableName.InsertMode))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateInsertResion()
        {
            var table = TableName.InsertResion;
            Create.Table(nameof(TableName.InsertResion))
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateBlance()
        {
            var table = TableName.Balance;
            Create.Table(nameof(TableName.Balance))
                .WithColumn("Id").AsInt64().PrimaryKey(NamingHelper.Pk(table)).NotNullable().Identity()
                .WithColumn("RequestDraffId").AsInt64().Nullable()//relation??
                .WithColumn($"{TableName.Invoice}Id").AsInt64().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Invoice,table),nameof(TableName.Invoice))
        }
    }
}insert resion
    request draff
