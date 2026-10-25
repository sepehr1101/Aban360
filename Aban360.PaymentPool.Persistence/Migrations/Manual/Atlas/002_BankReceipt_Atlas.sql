-- Connect to the intended SQL Server and SELECT Atlas as the current database before running.
-- This script never creates a database, switches to Aban360, or deletes application data.
SET NOCOUNT ON;
SET XACT_ABORT ON;
IF DB_NAME() <> N'Atlas'
    THROW 51010, N'این اسکریپت فقط در دیتابیس Atlas قابل اجرا است.', 1;
IF (SELECT compatibility_level FROM sys.databases WHERE database_id=DB_ID()) < 130
    THROW 51011, N'این اسکریپت به SQL Server با compatibility level حداقل 130 نیاز دارد.', 1;
BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'PaymentPool.Bank', N'U') IS NULL OR OBJECT_ID(N'PaymentPool.Uploader', N'U') IS NULL
       OR OBJECT_ID(N'PaymentPool.Credit', N'U') IS NULL
        THROW 51012, N'ابتدا اسکریپت 001 را اجرا کنید یا ساختار پایه PaymentPool را آماده کنید.', 1;

    IF OBJECT_ID(N'PaymentPool.BankReceiptImport', N'U') IS NULL
        CREATE TABLE PaymentPool.BankReceiptImport (
            Id uniqueidentifier NOT NULL PRIMARY KEY, UserId uniqueidentifier NOT NULL,
            FileHash varchar(64) NOT NULL, Status int NOT NULL, Version int NOT NULL, Payload nvarchar(max) NOT NULL,
            BankId smallint NOT NULL, UploadedAt datetime2 NOT NULL,
            IsDeleted bit NOT NULL CONSTRAINT DF_BankReceiptImport_IsDeleted DEFAULT(0),
            DeletedAt datetime2 NULL, DeletedBy uniqueidentifier NULL);

    IF COL_LENGTH(N'PaymentPool.BankReceiptImport', N'BankId') IS NULL
        ALTER TABLE PaymentPool.BankReceiptImport ADD BankId smallint NULL;
    IF COL_LENGTH(N'PaymentPool.BankReceiptImport', N'UploadedAt') IS NULL
        ALTER TABLE PaymentPool.BankReceiptImport ADD UploadedAt datetime2 NULL;
    IF COL_LENGTH(N'PaymentPool.BankReceiptImport', N'IsDeleted') IS NULL
        ALTER TABLE PaymentPool.BankReceiptImport ADD IsDeleted bit NOT NULL CONSTRAINT DF_BankReceiptImport_IsDeleted DEFAULT(0);
    IF COL_LENGTH(N'PaymentPool.BankReceiptImport', N'DeletedAt') IS NULL
        ALTER TABLE PaymentPool.BankReceiptImport ADD DeletedAt datetime2 NULL;
    IF COL_LENGTH(N'PaymentPool.BankReceiptImport', N'DeletedBy') IS NULL
        ALTER TABLE PaymentPool.BankReceiptImport ADD DeletedBy uniqueidentifier NULL;

    -- Dynamic SQL compiles after adding columns to an existing table.
    EXEC(N'UPDATE PaymentPool.BankReceiptImport
        SET BankId=COALESCE(BankId,CAST(JSON_VALUE(Payload,''$.BankId'') AS smallint)),
            UploadedAt=COALESCE(UploadedAt,CAST(JSON_VALUE(Payload,''$.CreatedAt'') AS datetime2));');
    ALTER TABLE PaymentPool.BankReceiptImport ALTER COLUMN BankId smallint NOT NULL;
    ALTER TABLE PaymentPool.BankReceiptImport ALTER COLUMN UploadedAt datetime2 NOT NULL;

    IF OBJECT_ID(N'PaymentPool.BankReceiptPaymentKey', N'U') IS NULL
        CREATE TABLE PaymentPool.BankReceiptPaymentKey (
            Hash varchar(64) NOT NULL PRIMARY KEY,
            ImportId uniqueidentifier NOT NULL REFERENCES PaymentPool.BankReceiptImport(Id), LineNumber int NOT NULL);

    IF OBJECT_ID(N'PaymentPool.BankReceiptDiscrepancy', N'U') IS NULL
        CREATE TABLE PaymentPool.BankReceiptDiscrepancy (
            ImportId uniqueidentifier NOT NULL REFERENCES PaymentPool.BankReceiptImport(Id),
            LineNumber int NOT NULL, UserId uniqueidentifier NOT NULL, UploadedAt datetime2 NOT NULL,
            IsActive bit NOT NULL, Snapshot nvarchar(max) NOT NULL, Description nvarchar(2000) NULL,
            DescribedBy uniqueidentifier NULL, DescribedAt datetime2 NULL,
            CONSTRAINT PK_BankReceiptDiscrepancy PRIMARY KEY(ImportId,LineNumber));

    IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'PaymentPool.BankReceiptDiscrepancy')
        AND name=N'CK_BankReceiptDiscrepancy_Description')
        ALTER TABLE PaymentPool.BankReceiptDiscrepancy ADD CONSTRAINT CK_BankReceiptDiscrepancy_Description
            CHECK ((Description IS NULL AND DescribedBy IS NULL AND DescribedAt IS NULL)
                OR (Description IS NOT NULL AND LEN(LTRIM(RTRIM(Description)))>0 AND DescribedBy IS NOT NULL AND DescribedAt IS NOT NULL));

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'PaymentPool.BankReceiptImport') AND name=N'IX_BankReceiptImport_User_Bank_Date')
        EXEC(N'CREATE INDEX IX_BankReceiptImport_User_Bank_Date ON PaymentPool.BankReceiptImport(UserId,BankId,IsDeleted,UploadedAt);');
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'PaymentPool.BankReceiptDiscrepancy') AND name=N'IX_BankReceiptDiscrepancy_User_UploadedAt')
        CREATE INDEX IX_BankReceiptDiscrepancy_User_UploadedAt ON PaymentPool.BankReceiptDiscrepancy(UserId,UploadedAt);

    DECLARE @constraintName sysname;
    SELECT @constraintName=name FROM sys.key_constraints
    WHERE parent_object_id=OBJECT_ID(N'PaymentPool.BankReceiptImport') AND type='UQ'
        AND INDEX_COL(N'PaymentPool.BankReceiptImport',unique_index_id,1)='FileHash';
    IF @constraintName IS NOT NULL
        EXEC(N'ALTER TABLE PaymentPool.BankReceiptImport DROP CONSTRAINT '+QUOTENAME(@constraintName));
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'PaymentPool.BankReceiptImport') AND name=N'UX_BankReceiptImport_ActiveFileHash')
        EXEC(N'CREATE UNIQUE INDEX UX_BankReceiptImport_ActiveFileHash ON PaymentPool.BankReceiptImport(FileHash) WHERE IsDeleted=0;');

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
                WHERE r.LineNumber >= 2 AND reasons.Reasons <> '[]' AND NOT EXISTS (SELECT 1 FROM PaymentPool.BankReceiptDiscrepancy d WHERE d.ImportId=i.Id AND d.LineNumber=r.LineNumber);

    EXEC(N'CREATE OR ALTER TRIGGER [PaymentPool].[TR_BankReceiptDiscrepancy_ImmutableDescription]
                ON [PaymentPool].[BankReceiptDiscrepancy] AFTER UPDATE, DELETE AS
                BEGIN
                    SET NOCOUNT ON;
                    IF EXISTS (SELECT 1 FROM deleted WHERE Description IS NOT NULL)
                       AND (UPDATE(Description) OR UPDATE(DescribedBy) OR UPDATE(DescribedAt)
                            OR NOT EXISTS (SELECT 1 FROM inserted))
                        THROW 51001, N''شرح ثبت‌شده قابل تغییر یا حذف نیست.'', 1;
                END');

    IF OBJECT_ID(N'PaymentPool.ManualSchemaVersion', N'U') IS NULL
        CREATE TABLE PaymentPool.ManualSchemaVersion (ScriptName nvarchar(150) NOT NULL PRIMARY KEY, AppliedAt datetime2 NOT NULL);
    IF NOT EXISTS (SELECT 1 FROM PaymentPool.ManualSchemaVersion WHERE ScriptName=N'002_BankReceipt_Atlas_v1')
        INSERT PaymentPool.ManualSchemaVersion VALUES(N'002_BankReceipt_Atlas_v1',SYSUTCDATETIME());
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
