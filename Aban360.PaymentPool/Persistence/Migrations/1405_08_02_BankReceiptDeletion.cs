using FluentMigrator;

namespace Aban360.PaymentPool.Persistence.Migrations
{
    [Migration(1405080202)]
    public sealed class BankReceiptDeletionDesign : Migration
    {
        public override void Up()
        {
            Alter.Table("BankReceiptImport").InSchema("PaymentPool")
                .AddColumn("BankId").AsInt16().Nullable()
                .AddColumn("UploadedAt").AsDateTime2().Nullable()
                .AddColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("DeletedAt").AsDateTime2().Nullable()
                .AddColumn("DeletedBy").AsGuid().Nullable();
            Execute.Sql(@"
                UPDATE [PaymentPool].[BankReceiptImport]
                SET BankId=CAST(JSON_VALUE(Payload, '$.BankId') AS smallint),
                    UploadedAt=CAST(JSON_VALUE(Payload, '$.CreatedAt') AS datetime2);");
            Alter.Column("BankId").OnTable("BankReceiptImport").InSchema("PaymentPool").AsInt16().NotNullable();
            Alter.Column("UploadedAt").OnTable("BankReceiptImport").InSchema("PaymentPool").AsDateTime2().NotNullable();
            Create.Index("IX_BankReceiptImport_User_Bank_Date").OnTable("BankReceiptImport").InSchema("PaymentPool")
                .OnColumn("UserId").Ascending().OnColumn("BankId").Ascending()
                .OnColumn("IsDeleted").Ascending().OnColumn("UploadedAt").Ascending();
            // Retain audit history while allowing a deleted file to be uploaded again.
            Execute.Sql(@"
                DECLARE @constraintName sysname;
                SELECT @constraintName=name FROM sys.key_constraints
                WHERE parent_object_id=OBJECT_ID(N'[PaymentPool].[BankReceiptImport]') AND type='UQ'
                    AND INDEX_COL(N'[PaymentPool].[BankReceiptImport]', unique_index_id, 1)='FileHash';
                IF @constraintName IS NOT NULL
                    EXEC(N'ALTER TABLE [PaymentPool].[BankReceiptImport] DROP CONSTRAINT ' + QUOTENAME(@constraintName));
                CREATE UNIQUE INDEX UX_BankReceiptImport_ActiveFileHash
                    ON [PaymentPool].[BankReceiptImport](FileHash) WHERE IsDeleted=0;");
        }
        public override void Down()
        {
            Execute.Sql(@"
                IF EXISTS (SELECT FileHash FROM [PaymentPool].[BankReceiptImport] GROUP BY FileHash HAVING COUNT(*)>1)
                    THROW 51002, N'به علت بارگذاری مجدد فایل‌های حذف‌شده، بازگشت این migration نیازمند بررسی سوابق است.', 1;
                DROP INDEX UX_BankReceiptImport_ActiveFileHash ON [PaymentPool].[BankReceiptImport];
                ALTER TABLE [PaymentPool].[BankReceiptImport] ADD CONSTRAINT UQ_BankReceiptImport_FileHash UNIQUE(FileHash);");
            Delete.Index("IX_BankReceiptImport_User_Bank_Date").OnTable("BankReceiptImport").InSchema("PaymentPool");
            Delete.Column("BankId").FromTable("BankReceiptImport").InSchema("PaymentPool");
            Delete.Column("UploadedAt").FromTable("BankReceiptImport").InSchema("PaymentPool");
            Delete.Column("IsDeleted").FromTable("BankReceiptImport").InSchema("PaymentPool");
            Delete.Column("DeletedAt").FromTable("BankReceiptImport").InSchema("PaymentPool");
            Delete.Column("DeletedBy").FromTable("BankReceiptImport").InSchema("PaymentPool");
        }
    }
}
