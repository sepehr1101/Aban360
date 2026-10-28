/*
Aban360 Ticketing -- standalone manual installation.
Equivalent to Up() of FluentMigrator migration 1405080401.

1. Take and verify a backup before running against the live database.
2. Select the application database in SSMS and replace the expected name below.
3. FILESTREAM and its writable default filegroup/container must already exist.
4. Run this WHOLE file once, outside another transaction.

Creates only five NEW UserPool ticketing tables and their initial statuses.
Does not alter/delete existing application tables, run other migrations/seeds,
change SQL Server configuration, create a database, or update migration history.
If any target object already exists, stops without replacing it.
Future automatic migrations must reconcile version 1405080401 before being enabled.
*/
SET NOCOUNT ON;

-- Check this FIRST. RAISERROR does not honor XACT_ABORT; THROW can abort an
-- already-open caller transaction when that session has XACT_ABORT ON.
IF @@TRANCOUNT <> 0
BEGIN
    RAISERROR ('Run outside an existing transaction. The caller transaction has not been changed.', 16, 1);
    RETURN;
END;

DECLARE @ExpectedDatabase sysname = N'َAban360';
IF @ExpectedDatabase IS NULL OR DB_NAME() <> @ExpectedDatabase
    THROW 51100, 'Select the application database and set @ExpectedDatabase to its exact name.', 1;
IF DB_NAME() IN (N'master', N'model', N'msdb', N'tempdb')
    THROW 51101, 'Installation in a system database is not allowed.', 1;
IF (2 & @@OPTIONS) = 2
    THROW 51107, 'Run SET IMPLICIT_TRANSACTIONS OFF in a separate query, then run this file again.', 1;
IF COALESCE(CONVERT(nvarchar(60), DATABASEPROPERTYEX(DB_NAME(), N'Updateability')), N'') <> N'READ_WRITE'
    THROW 51108, 'The application database must be writable.', 1;
IF SCHEMA_ID(N'UserPool') IS NULL OR OBJECT_ID(N'[UserPool].[User]', N'U') IS NULL
    THROW 51103, 'The existing UserPool schema and User table are required.', 1;
IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID(N'[UserPool].[User]')
                 AND [name] = N'Id' AND system_type_id = TYPE_ID(N'uniqueidentifier') AND is_nullable = 0)
    THROW 51104, 'UserPool.User.Id must be a non-nullable uniqueidentifier.', 1;
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes AS i
    JOIN sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
    JOIN sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
    WHERE i.object_id = OBJECT_ID(N'[UserPool].[User]')
      AND i.is_unique = 1 AND i.is_disabled = 0 AND i.is_hypothetical = 0 AND i.has_filter = 0
      AND ic.key_ordinal = 1 AND c.[name] = N'Id'
      AND NOT EXISTS (SELECT 1 FROM sys.index_columns AS other_key
                      WHERE other_key.object_id = i.object_id AND other_key.index_id = i.index_id
                        AND other_key.key_ordinal > 1))
    THROW 51109, 'UserPool.User.Id requires an enabled, unfiltered single-key unique index or primary key.', 1;

DECLARE @OriginalXactAbort bit = CASE WHEN (16384 & @@OPTIONS) = 16384 THEN 1 ELSE 0 END;
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @LockResult int;
    EXEC @LockResult = sys.sp_getapplock
        @Resource = N'Aban360.UserPool.Ticketing.Install',
        @LockMode = N'Exclusive', @LockOwner = N'Transaction', @LockTimeout = 0;
    IF @LockResult < 0
        THROW 51105, 'Another ticketing installation is in progress.', 1;

    IF EXISTS (SELECT 1 FROM sys.objects
               WHERE schema_id = SCHEMA_ID(N'UserPool')
                 AND [name] IN (N'TicketStatus', N'Ticket', N'TicketMessage', N'TicketAttachment', N'TicketRating'))
        THROW 51106, 'A target object already exists. Inspect the schema; nothing will be overwritten.', 1;

    IF COALESCE(CONVERT(int, SERVERPROPERTY('FilestreamEffectiveLevel')), 0) = 0
        THROW 51000, 'Ticketing requires FILESTREAM to be enabled on SQL Server.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM sys.filegroups AS g
        JOIN sys.database_files AS f ON f.data_space_id = g.data_space_id
        WHERE g.[type] = 'FD' AND g.is_default = 1 AND g.is_read_only = 0
          AND f.[type] = 2 AND f.[state] = 0)
        THROW 51001, 'Ticketing requires a writable default FILESTREAM filegroup with an online container.', 1;

    -- Execute DDL at a lower scope so compilation/name-resolution errors reach CATCH.
    DECLARE @CreateTablesSql nvarchar(max) = N'
    CREATE TABLE [UserPool].[TicketStatus]
    (
        [Id] SMALLINT NOT NULL CONSTRAINT [PK_TicketStatus] PRIMARY KEY,
        [Code] NVARCHAR(50) NOT NULL CONSTRAINT [UQ_TicketStatus_Code] UNIQUE,
        [Title] NVARCHAR(100) NOT NULL
    );

    INSERT INTO [UserPool].[TicketStatus] ([Id], [Code], [Title])
    VALUES (1, N''New'', N''جدید''),
           (2, N''InProgress'', N''در حال بررسی''),
           (3, N''Answered'', N''پاسخ داده‌شده''),
           (4, N''Closed'', N''بسته‌شده'');

    CREATE TABLE [UserPool].[Ticket]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Ticket] PRIMARY KEY,
        [TicketNumber] NVARCHAR(50) NOT NULL CONSTRAINT [UQ_Ticket_TicketNumber] UNIQUE,
        [Subject] NVARCHAR(255) NOT NULL,
        [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
        [AssignedToUserId] UNIQUEIDENTIFIER NULL,
        [StatusId] SMALLINT NOT NULL CONSTRAINT [DF_Ticket_StatusId] DEFAULT (1),
        [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Ticket_CreatedAt] DEFAULT SYSUTCDATETIME(),
        [UpdatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_Ticket_UpdatedAt] DEFAULT SYSUTCDATETIME(),
        [ClosedAt] DATETIME2 NULL,
        CONSTRAINT [FK_Ticket_CreatedByUser] FOREIGN KEY ([CreatedByUserId]) REFERENCES [UserPool].[User] ([Id]),
        CONSTRAINT [FK_Ticket_AssignedToUser] FOREIGN KEY ([AssignedToUserId]) REFERENCES [UserPool].[User] ([Id]),
        CONSTRAINT [FK_Ticket_Status] FOREIGN KEY ([StatusId]) REFERENCES [UserPool].[TicketStatus] ([Id])
    );

    CREATE TABLE [UserPool].[TicketMessage]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_TicketMessage] PRIMARY KEY,
        [TicketId] BIGINT NOT NULL,
        [SenderUserId] UNIQUEIDENTIFIER NOT NULL,
        [Body] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_TicketMessage_CreatedAt] DEFAULT SYSUTCDATETIME(),
        CONSTRAINT [FK_TicketMessage_Ticket] FOREIGN KEY ([TicketId]) REFERENCES [UserPool].[Ticket] ([Id]),
        CONSTRAINT [FK_TicketMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [UserPool].[User] ([Id])
    );

    CREATE TABLE [UserPool].[TicketAttachment]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_TicketAttachment] PRIMARY KEY,
        [TicketMessageId] BIGINT NOT NULL,
        [FileGuid] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL CONSTRAINT [DF_TicketAttachment_FileGuid] DEFAULT NEWID(),
        [OriginalFileName] NVARCHAR(255) NOT NULL,
        [ContentType] NVARCHAR(150) NOT NULL,
        [SizeBytes] BIGINT NOT NULL,
        [FileContent] VARBINARY(MAX) FILESTREAM NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_TicketAttachment_CreatedAt] DEFAULT SYSUTCDATETIME(),
        CONSTRAINT [UQ_TicketAttachment_FileGuid] UNIQUE ([FileGuid]),
        CONSTRAINT [CK_TicketAttachment_SizeBytes] CHECK ([SizeBytes] >= 0),
        CONSTRAINT [FK_TicketAttachment_TicketMessage] FOREIGN KEY ([TicketMessageId]) REFERENCES [UserPool].[TicketMessage] ([Id])
    );

    CREATE TABLE [UserPool].[TicketRating]
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_TicketRating] PRIMARY KEY,
        [TicketMessageId] BIGINT NOT NULL,
        [RatedByUserId] UNIQUEIDENTIFIER NOT NULL,
        [Score] TINYINT NOT NULL,
        [Comment] NVARCHAR(1023) NULL,
        [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_TicketRating_CreatedAt] DEFAULT SYSUTCDATETIME(),
        CONSTRAINT [UQ_TicketRating_TicketMessageId] UNIQUE ([TicketMessageId]),
        CONSTRAINT [CK_TicketRating_Score] CHECK ([Score] BETWEEN 1 AND 5),
        CONSTRAINT [FK_TicketRating_TicketMessage] FOREIGN KEY ([TicketMessageId]) REFERENCES [UserPool].[TicketMessage] ([Id]),
        CONSTRAINT [FK_TicketRating_RatedByUser] FOREIGN KEY ([RatedByUserId]) REFERENCES [UserPool].[User] ([Id])
    );

    CREATE INDEX [IX_Ticket_CreatedByUserId] ON [UserPool].[Ticket] ([CreatedByUserId]);
    CREATE INDEX [IX_Ticket_AssignedToUserId] ON [UserPool].[Ticket] ([AssignedToUserId]);
    CREATE INDEX [IX_Ticket_StatusId_CreatedAt] ON [UserPool].[Ticket] ([StatusId], [CreatedAt]);
    CREATE INDEX [IX_Ticket_CreatedAt] ON [UserPool].[Ticket] ([CreatedAt]);
    CREATE INDEX [IX_TicketMessage_TicketId_CreatedAt] ON [UserPool].[TicketMessage] ([TicketId], [CreatedAt], [Id]);
    CREATE INDEX [IX_TicketMessage_SenderUserId] ON [UserPool].[TicketMessage] ([SenderUserId]);
    CREATE INDEX [IX_TicketAttachment_TicketMessageId] ON [UserPool].[TicketAttachment] ([TicketMessageId]);
    CREATE INDEX [IX_TicketRating_RatedByUserId] ON [UserPool].[TicketRating] ([RatedByUserId]);';
    EXEC sys.sp_executesql @CreateTablesSql;

    -- Validate the new structure BEFORE committing it.
    IF (SELECT COUNT(*) FROM sys.tables
        WHERE schema_id = SCHEMA_ID(N'UserPool')
          AND [name] IN (N'TicketStatus', N'Ticket', N'TicketMessage', N'TicketAttachment', N'TicketRating')) <> 5
        THROW 51110, 'Installation validation failed: expected five tables.', 1;

    IF (SELECT COUNT(*) FROM sys.foreign_keys
        WHERE parent_object_id IN (OBJECT_ID(N'[UserPool].[Ticket]'), OBJECT_ID(N'[UserPool].[TicketMessage]'),
                                   OBJECT_ID(N'[UserPool].[TicketAttachment]'), OBJECT_ID(N'[UserPool].[TicketRating]'))
          AND is_disabled = 0 AND is_not_trusted = 0) <> 8
        THROW 51111, 'Installation validation failed: expected eight enabled, trusted foreign keys.', 1;

    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[UserPool].[TicketAttachment]')
                   AND [name] = N'FileContent' AND is_filestream = 1)
       OR NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[UserPool].[TicketAttachment]')
                       AND [name] = N'FileGuid' AND is_rowguidcol = 1)
        THROW 51112, 'Installation validation failed: FILESTREAM or ROWGUIDCOL is missing.', 1;

    -- Compile this query only after the table exists, so errors are caught here.
    EXEC sys.sp_executesql N'IF (SELECT COUNT(*) FROM [UserPool].[TicketStatus]) <> 4
        THROW 51113, ''Installation validation failed: expected four initial statuses.'', 1;';

    COMMIT TRANSACTION;
    IF @OriginalXactAbort = 0 SET XACT_ABORT OFF;
    PRINT N'Ticketing installation committed successfully.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    IF @OriginalXactAbort = 0 SET XACT_ABORT OFF;
    THROW;
END CATCH;

-- Read-only verification. Expected: five tables, eight foreign keys,
-- FILESTREAM = 1 on FileContent, ROWGUIDCOL = 1 on FileGuid, four statuses.
SELECT s.[name] AS SchemaName, t.[name] AS TableName
FROM sys.tables AS t JOIN sys.schemas AS s ON s.schema_id = t.schema_id
WHERE s.[name] = N'UserPool'
  AND t.[name] IN (N'TicketStatus', N'Ticket', N'TicketMessage', N'TicketAttachment', N'TicketRating')
ORDER BY t.[name];

SELECT fk.[name] AS ForeignKeyName,
       OBJECT_NAME(fk.parent_object_id) AS SourceTable,
       COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS SourceColumn,
       OBJECT_SCHEMA_NAME(fk.referenced_object_id) AS TargetSchema,
       OBJECT_NAME(fk.referenced_object_id) AS TargetTable,
       fk.is_disabled, fk.is_not_trusted
FROM sys.foreign_keys AS fk
JOIN sys.foreign_key_columns AS fkc ON fkc.constraint_object_id = fk.object_id
WHERE OBJECT_SCHEMA_NAME(fk.parent_object_id) = N'UserPool'
  AND OBJECT_NAME(fk.parent_object_id) IN (N'Ticket', N'TicketMessage', N'TicketAttachment', N'TicketRating')
ORDER BY SourceTable, ForeignKeyName;

SELECT [name], is_filestream, is_rowguidcol
FROM sys.columns WHERE object_id = OBJECT_ID(N'[UserPool].[TicketAttachment]')
  AND [name] IN (N'FileContent', N'FileGuid');
SELECT [Id], [Code], [Title] FROM [UserPool].[TicketStatus] ORDER BY [Id];
