using FluentMigrator;

namespace Aban360.PaymentPool.Persistence.Migrations
{
    [Migration(1405080201)]
    public sealed class BankReceiptDiscrepancyDesign : Migration
    {
        public override void Up()
        {
            Create.Table("BankReceiptDiscrepancy").InSchema("PaymentPool")
                .WithColumn("ImportId").AsGuid().PrimaryKey("PK_BankReceiptDiscrepancy")
                    .ForeignKey("FK_BankReceiptDiscrepancy_Import", "PaymentPool", "BankReceiptImport", "Id")
                .WithColumn("LineNumber").AsInt32().PrimaryKey("PK_BankReceiptDiscrepancy")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UploadedAt").AsDateTime2().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable()
                .WithColumn("Snapshot").AsString(int.MaxValue).NotNullable()
                .WithColumn("Description").AsString(2000).Nullable()
                .WithColumn("DescribedBy").AsGuid().Nullable()
                .WithColumn("DescribedAt").AsDateTime2().Nullable();
            Create.Index("IX_BankReceiptDiscrepancy_User_UploadedAt").OnTable("BankReceiptDiscrepancy").InSchema("PaymentPool")
                .OnColumn("UserId").Ascending().OnColumn("UploadedAt").Ascending();
            Execute.Sql(@"
                ALTER TABLE [PaymentPool].[BankReceiptDiscrepancy]
                ADD CONSTRAINT CK_BankReceiptDiscrepancy_Description
                CHECK ((Description IS NULL AND DescribedBy IS NULL AND DescribedAt IS NULL)
                    OR (Description IS NOT NULL AND LEN(LTRIM(RTRIM(Description))) > 0
                        AND DescribedBy IS NOT NULL AND DescribedAt IS NOT NULL));");
            // Project existing parsed rows so previously uploaded files appear in the report too.
            Execute.Sql(@"
                INSERT INTO [PaymentPool].[BankReceiptDiscrepancy]
                    (ImportId, LineNumber, UserId, UploadedAt, IsActive, Snapshot)
                SELECT i.Id, r.LineNumber, i.UserId,
                    CAST(JSON_VALUE(i.Payload, '$.CreatedAt') AS datetime2), 1,
                    (SELECT i.Id AS ImportId, r.LineNumber,
                        JSON_VALUE(i.Payload, '$.CreatedAt') AS UploadedAt,
                        JSON_VALUE(i.Payload, '$.BankName') AS BankName,
                        JSON_VALUE(i.Payload, '$.FileName') AS FileName,
                        r.BillId, r.PaymentId, r.PayDate, r.Amount, ISNULL(r.RawData, '') AS RawData,
                        CAST(1 AS bit) AS IsActive, JSON_QUERY(reasons.Reasons) AS Reasons
                     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
                FROM [PaymentPool].[BankReceiptImport] i
                CROSS APPLY OPENJSON(i.Payload, '$.Rows') WITH
                    (LineNumber int, BillId nvarchar(13), PaymentId nvarchar(13), PayDate nvarchar(8),
                     Amount bigint, RawData nvarchar(max), Issues nvarchar(max) AS JSON) r
                CROSS APPLY
                    (SELECT (SELECT reason.Code, reason.Message, r.LineNumber AS LineNumber FROM
                        (SELECT Code, Message FROM OPENJSON(r.Issues) WITH (Code nvarchar(200), Message nvarchar(max))
                         UNION ALL
                         SELECT Code, Message FROM OPENJSON(i.Payload, '$.Issues')
                            WITH (Code nvarchar(200), Message nvarchar(max), LineNumber int) f
                            WHERE f.LineNumber IS NULL OR f.LineNumber=1 OR f.LineNumber=r.LineNumber) reason
                     FOR JSON PATH) AS Reasons) reasons
                WHERE r.LineNumber >= 2 AND reasons.Reasons <> '[]';");
            Execute.Sql(@"
                CREATE TRIGGER [PaymentPool].[TR_BankReceiptDiscrepancy_ImmutableDescription]
                ON [PaymentPool].[BankReceiptDiscrepancy] AFTER UPDATE, DELETE AS
                BEGIN
                    SET NOCOUNT ON;
                    IF EXISTS (SELECT 1 FROM deleted WHERE Description IS NOT NULL)
                       AND (UPDATE(Description) OR UPDATE(DescribedBy) OR UPDATE(DescribedAt)
                            OR NOT EXISTS (SELECT 1 FROM inserted))
                        THROW 51001, N'شرح ثبت‌شده قابل تغییر یا حذف نیست.', 1;
                END");
        }
        public override void Down()
        {
            Delete.Table("BankReceiptDiscrepancy").InSchema("PaymentPool");
        }
    }
}
