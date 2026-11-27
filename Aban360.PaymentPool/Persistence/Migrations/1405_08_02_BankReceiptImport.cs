using FluentMigrator;

namespace Aban360.PaymentPool.Persistence.Migrations
{
    [Migration(14050802)]
    public sealed class BankReceiptImportDesign : Migration
    {
        public override void Up()
        {
            Create.Table("BankReceiptImport").InSchema("PaymentPool")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("FileHash").AsAnsiString(64).NotNullable().Unique()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Payload").AsString(int.MaxValue).NotNullable();
            Create.Table("BankReceiptPaymentKey").InSchema("PaymentPool")
                .WithColumn("Hash").AsAnsiString(64).PrimaryKey()
                .WithColumn("ImportId").AsGuid().NotNullable()
                    .ForeignKey("FK_BankReceiptPaymentKey_Import", "PaymentPool", "BankReceiptImport", "Id")
                .WithColumn("LineNumber").AsInt32().NotNullable();
        }
        public override void Down()
        {
            Delete.Table("BankReceiptPaymentKey").InSchema("PaymentPool");
            Delete.Table("BankReceiptImport").InSchema("PaymentPool");
        }
    }
}
