using Aban360.CalculationPool.Persistence.Migrations.Enums;
using Aban360.CalculationPool.Persistence.Extensions;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CalculationPool.Persistence.Migrations
{
    [Migration(1403120501)]
    public class DbInitialDesign: Migration
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
        private void CreateOfferingUnit()
        {
            var table = TableName.OfferingUnit;
            Create.Table(nameof(TableName.OfferingUnit))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Symbol").AsString(_255).NotNullable();
        }
        private void CreateOfferingGroup()
        {
            var table = TableName.OfferingGroup;
            Create.Table(nameof(TableName.OfferingGroup))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateOffering()
        {
            var table = TableName.Offering;
            Create.Table(nameof(TableName.Offering))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.OfferingUnit)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingUnit, TableName.Offering), nameof(TableName.OfferingUnit), Id)
                .WithColumn($"{nameof(TableName.OfferingGroup)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingGroup, TableName.Offering), nameof(TableName.OfferingGroup), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("InstallmentOption").AsBoolean().NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoiceType()
        {
            var table = TableName.InvoiceType;
            Create.Table(nameof(TableName.InvoiceType))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoiceStatus()
        {
            var table = TableName.InvoiceStatus;
            Create.Table(nameof(TableName.InvoiceStatus))
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoinceLineItemInsertMode()
        {
            var table = TableName.InvoinceLineItemInsertMode;
            Create.Table(nameof(TableName.InvoinceLineItemInsertMode))
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoice()
        {
            var table = TableName.Invoice;
            Create.Table(nameof(TableName.Invoice))
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("InvoiceTypeId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoiceType, TableName.Invoice), nameof(TableName.InvoiceType), Id)
                .WithColumn("InvoiceStatusId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoiceStatus, TableName.Invoice), nameof(TableName.InvoiceStatus), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("OfferingCount").AsInt16().NotNullable()
                .WithColumn("DepositRate").AsInt16().NotNullable()
                .WithColumn("InstallmentCount").AsInt16().NotNullable();
        }
        private void CreateInvoiceLineItem()
        {
            var table = TableName.InvoiceLineItem;
            Create.Table(nameof(TableName.InvoiceLineItem))
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("InvoiceId").AsInt64().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Invoice, TableName.InvoiceLineItem), nameof(TableName.Invoice), Id)
                .WithColumn("OfferingId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Offering, TableName.InvoiceLineItem), nameof(TableName.Offering), Id)
                .WithColumn("InvoinceLineItemInsertModeId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoinceLineItemInsertMode, TableName.InvoiceLineItem), nameof(TableName.InvoinceLineItemInsertMode), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("Quanity").AsInt32().NotNullable();
        }
        private void CreateInvoiceInstallment()
        {
            var table= TableName.InvoiceInstallment;
            Create.Table(nameof(TableName.InvoiceInstallment))
                .WithColumn (Id).AsInt64().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("InvoiceId").AsInt64().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Invoice, TableName.InvoiceInstallment), nameof(TableName.Invoice), Id)
                .WithColumn("Amount").AsInt64().NotNullable() 
                .WithColumn("DueDateJalali").AsAnsiString(10).NotNullable()
                .WithColumn("DueDateTime").AsDateTime().NotNullable() 
                .WithColumn("InstallmentOrder").AsInt32().NotNullable()
                .WithColumn("BillId").AsAnsiString(20).Nullable()
                .WithColumn("PaymentId").AsAnsiString(20).Nullable();
        }
        //private void CreateInvoiceStatus()
    }
}
