using Aban360.CalculationPool.Persistence.Constants;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.CalculationPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.CalculationPool.Persistence.Migrations
{
    [Migration(1403120501)]
    public class DbInitialDesign: Migration
    {
        string _schema= TableSchema.Name , Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023;
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
        private void CreateOfferingUnit()
        {
            var table = TableName.OfferingUnit;
            Create.Table(nameof(TableName.OfferingUnit)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Symbol").AsString(_255).NotNullable();
        }
        private void CreateOfferingGroup()
        {
            var table = TableName.OfferingGroup;
            Create.Table(nameof(TableName.OfferingGroup)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateOffering()
        {
            var table = TableName.Offering;
            Create.Table(nameof(TableName.Offering)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.OfferingUnit)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingUnit, TableName.Offering),_schema, nameof(TableName.OfferingUnit), Id)
                .WithColumn($"{nameof(TableName.OfferingGroup)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OfferingGroup, TableName.Offering), _schema, nameof(TableName.OfferingGroup), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("InstallmentOption").AsBoolean().NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateTariffCalculationMode()
        {
            var table = TableName.TariffCalculationMode;
            Create.Table(nameof(TableName.TariffCalculationMode)).InSchema(_schema)
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateCompanyServiceType()
        {
            var table = TableName.CompanyServiceType;
            Create.Table(nameof(TableName.CompanyServiceType)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.TariffCalculationMode}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.TariffCalculationMode, TableName.CompanyServiceType), _schema,nameof(TableName.TariffCalculationMode), Id)
                .WithColumn("Description").AsString(_1023).Nullable();
        }// foroosh, ab baha, pas as foroosh
        private void CreateCompanyService()// foroosh ab, fazelab, taqir qotr ,...
        {
            var table = TableName.CompanyService;
            Create.Table(nameof(TableName.CompanyService)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("IsMultiSelect").AsBoolean().NotNullable()
                .WithColumn($"{nameof(TableName.CompanyServiceType)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.CompanyServiceType, table), _schema, nameof(TableName.CompanyServiceType), Id);
        }
        private void CreateCompanyServiceOffering()
        {
            var table = TableName.CompanyServiceOffering;
            Create.Table(nameof(TableName.CompanyServiceOffering)).InSchema(_schema)
                .WithColumn($"{nameof(TableName.CompanyService)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.CompanyService, table), _schema, nameof(TableName.CompanyService), Id)
                .WithColumn($"{nameof(TableName.Offering)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Offering, table), _schema, nameof(TableName.Offering), Id)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table));
        }

        private void CreateInvoiceType()
        {
            var table = TableName.InvoiceType;
            Create.Table(nameof(TableName.InvoiceType)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoiceStatus()
        {
            var table = TableName.InvoiceStatus;
            Create.Table(nameof(TableName.InvoiceStatus)).InSchema(_schema)
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateInvoiceLineItemInsertMode()
        {
            var table = TableName.InvoiceLineItemInsertMode;
            Create.Table(nameof(TableName.InvoiceLineItemInsertMode)).InSchema(_schema)
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }      
        private void CreateInvoice()
        {
            var table = TableName.Invoice;
            Create.Table(nameof(TableName.Invoice)).InSchema(_schema)
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("InvoiceTypeId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoiceType, TableName.Invoice), _schema, nameof(TableName.InvoiceType), Id)
                .WithColumn("InvoiceStatusId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoiceStatus, TableName.Invoice), _schema, nameof(TableName.InvoiceStatus), Id)
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("OfferingCount").AsInt16().NotNullable()
                .WithColumn("DepositRate").AsInt16().NotNullable()
                .WithColumn("InstallmentCount").AsInt16().NotNullable();
        }
        private void CreateInvoiceLineItem()
        {
            var table = TableName.InvoiceLineItem;
            Create.Table(nameof(TableName.InvoiceLineItem)).InSchema(_schema)
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("InvoiceId").AsInt64().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Invoice, TableName.InvoiceLineItem), _schema, nameof(TableName.Invoice), Id)
                .WithColumn("OfferingId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Offering, TableName.InvoiceLineItem), _schema, nameof(TableName.Offering), Id)
                .WithColumn("InvoiceLineItemInsertModeId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.InvoiceLineItemInsertMode, TableName.InvoiceLineItem), _schema, nameof(TableName.InvoiceLineItemInsertMode), Id)                     
                .WithColumn("Amount").AsInt64().NotNullable()
                .WithColumn("Quanity").AsInt32().NotNullable();
        }
        private void CreateInvoiceInstallment()
        {
            var table= TableName.InvoiceInstallment;
            Create.Table(nameof(TableName.InvoiceInstallment)).InSchema(_schema)
                .WithColumn (Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("InvoiceId").AsInt64().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.Invoice, table), _schema, nameof(TableName.Invoice), Id)
                .WithColumn("Amount").AsInt64().NotNullable() 
                .WithColumn("DueDateJalali").AsAnsiString(10).NotNullable()
                .WithColumn("DueDateTime").AsDateTime().NotNullable() 
                .WithColumn("InstallmentOrder").AsInt32().NotNullable()
                .WithColumn("BillId").AsAnsiString(20).Nullable()
                .WithColumn("PaymentId").AsAnsiString(20).Nullable();
        }
        private void CreateWaterMeterChangeNumberHistory()
        {
            var table = TableName.WaterMeterChangeNumberHistory;
            Create.Table(nameof(TableName.WaterMeterChangeNumberHistory)).InSchema(_schema)
                .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Consumption").AsInt64().NotNullable()//Todo: type
                .WithColumn("ConstumptionAverage").AsInt64().NotNullable()//Todo: type
                .WithColumn("ChangeMeterReasonId").AsInt16().NotNullable()
                .WithColumn($"{TableName.InvoiceInstallment}Id").AsInt64().NotNullable()//Todo
                    .ForeignKey(NamingHelper.Fk(TableName.InvoiceInstallment, table), _schema, nameof(TableName.InvoiceInstallment), Id);
        }
        private void CreateInvoiceEvent()
        {

        }
        //private void CreateInvoiceStatus()

        private void CreateImpactSign()
        {
            var table = TableName.ImpactSign;
            Create.Table(nameof(TableName.ImpactSign)).InSchema(_schema)
                .WithColumn("Id").AsInt16().NotNullable().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Multiplier").AsInt16().NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).Nullable();
        }
        private void CreateLineItemTypeGroup()
        {
            var table = TableName.LineItemTypeGroup;
            Create.Table(nameof(TableName.LineItemTypeGroup)).InSchema(_schema)
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("ImpactSignId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.ImpactSign, table), _schema, nameof(TableName.ImpactSign), Id)
               .WithColumn("Description").AsString(_1023).Nullable();
        }
        private void CreateLineItemType()
        {
            var table = TableName.LineItemType;
            Create.Table(nameof(TableName.LineItemType)).InSchema(_schema)
               .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
               .WithColumn("LineItemTypeGroupId").AsInt16().NotNullable()
                     .ForeignKey(NamingHelper.Fk(TableName.LineItemTypeGroup, table), _schema, nameof(TableName.LineItemTypeGroup), Id)
               .WithColumn("Title").AsString(_255).NotNullable()
               .WithColumn("Description").AsString(_1023).Nullable();
        }
      
        private void CreateTariffConstant()
        {
            var table = TableName.TariffConstant;
            Create.Table(nameof(TableName.TariffConstant)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Value").AsString(_255).NotNullable()
                .WithColumn("Condition").AsString(int.MaxValue).NotNullable()
                .WithColumn("Key").AsString(_255).NotNullable()
                .WithColumn("FromDateJalali").AsString(10).NotNullable()
                .WithColumn("ToDateJalali").AsString(10).NotNullable()
                .WithColumn("Description").AsString(_255).Nullable();
        }
        private void CreateTariff()
        {
            var table = TableName.Tariff;
            Create.Table(nameof(TableName.Tariff)).InSchema(_schema)
                  .WithColumn("Id").AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                  .WithColumn($"{TableName.LineItemType}Id").AsInt16().NotNullable()
                       .ForeignKey(NamingHelper.Fk(TableName.LineItemType, table), _schema, nameof(TableName.LineItemType), Id)
                  .WithColumn($"{TableName.Offering}Id").AsInt16().NotNullable()
                       .ForeignKey(NamingHelper.Fk(TableName.Offering, table), _schema, nameof(TableName.Offering), Id)
                  .WithColumn("Condition").AsString(int.MinValue).NotNullable()
                  .WithColumn("Formula").AsString(int.MaxValue).NotNullable()
                  .WithColumn("FromDateJalali").AsString(10).NotNullable()
                  .WithColumn("ToDateJalali").AsString(10).NotNullable()
                  .WithColumn("Description").AsString(_255).Nullable();
        }
        private void CreateSupportedOperator()
        {
            var table= TableName.SupportedOperator;
            Create.Table($"{nameof(TableName.SupportedOperator)}").InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Description").AsString(_1023).NotNullable();
        }
        private void CreateSupportedField()
        {
            var table= TableName.SupportedField;
            Create.Table($"{nameof(TableName.SupportedField)}").InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255)
                .WithColumn("Description").AsString(_1023).NotNullable();
        }
    }
}