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

        private void CreateMaliatMaaherWrapper()
        {
            var table = TableName.MaliatMaaherWrapper;

            Create.Table($"{nameof(TableName.MaliatMaaherWrapper)}").InSchema(_schema)
                .WithColumn(Id).AsInt32().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("InsertDateTime").AsDateTime().NotNullable()
                .WithColumn("InsertByUserId").AsGuid().NotNullable()
                .WithColumn("ConfirmedDateTime").AsDateTime().Nullable()
                .WithColumn("SendDateTime").AsDateTime().Nullable()
                .WithColumn("SendByUserId").AsGuid().Nullable()
                .WithColumn("InvoiceCount").AsInt32().Nullable()
                .WithColumn("SumAmount").AsInt64().Nullable();
        }
        private void CreateMaliatMaaherDetail()
        {
            var table = TableName.MaliatMaaherDetail;

            Create.Table($"{nameof(TableName.MaliatMaaherDetail)}").InSchema(_schema)
                .WithColumn(Id).AsInt64().NotNullable().Identity().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("WrapperId").AsInt32().NotNullable()
                .WithColumn("indatim").AsInt64().NotNullable()
                .WithColumn("inty").AsString(5).NotNullable()
                .WithColumn("inno").AsInt64().NotNullable()
                .WithColumn("inp").AsString(15).NotNullable()
                .WithColumn("ins").AsString(8).NotNullable()
                .WithColumn("tob").AsString(15).NotNullable()
                .WithColumn("bid").AsFixedLengthString(12).NotNullable()
                .WithColumn("bpc").AsFixedLengthString(12).NotNullable()
                .WithColumn("radif").AsInt32().NotNullable()
                .WithColumn("billId").AsFixedLengthString(13).NotNullable()
                .WithColumn("sstid").AsString(15).NotNullable()
                .WithColumn("sstt").AsString(47).NotNullable()
                .WithColumn("mu").AsString(4).NotNullable()
                .WithColumn("am").AsInt32().NotNullable()
                .WithColumn("fee").AsInt64().NotNullable()
                .WithColumn("dis").AsInt32().NotNullable()
                .WithColumn("town").AsDecimal(6, 0).NotNullable()
                .WithColumn("date_bed").AsFixedLengthString(10).NotNullable()
                .WithColumn("item1").AsInt64().NotNullable()
                .WithColumn("item2").AsInt64().NotNullable()
                .WithColumn("item3").AsInt64().NotNullable()
                .WithColumn("item4").AsInt64().NotNullable()
                .WithColumn("item5").AsInt64().NotNullable()
                .WithColumn("itemUnit1").AsFixedLengthString(4).NotNullable()
                .WithColumn("itemUnit2").AsFixedLengthString(4).NotNullable()
                .WithColumn("itemUnit3").AsFixedLengthString(4).NotNullable()
                .WithColumn("itemUnit4").AsFixedLengthString(4).NotNullable()
                .WithColumn("itemUnit5").AsFixedLengthString(4).NotNullable()
                .WithColumn("fetch_datetime").AsDateTime().Nullable()
                .WithColumn("send_datetime").AsDateTime().Nullable()
                .WithColumn("uuid").AsFixedLengthString(36).Nullable()
                .WithColumn("flow_state").AsInt32().NotNullable()
                .WithColumn("error_code").AsInt32().Nullable()
                .WithColumn("final_state").AsString(31).Nullable()
                .WithColumn("result").AsString(int.MaxValue).Nullable()
                .WithColumn("IsDelete").AsBoolean().Nullable()
                .WithColumn("TaxId").AsFixedLengthString(22).Nullable();
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
