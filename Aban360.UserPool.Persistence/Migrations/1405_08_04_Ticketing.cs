using Aban360.UserPool.Persistence.Constants;
using FluentMigrator;

namespace Aban360.UserPool.Persistence.Migrations
{
    [Migration(1405080401)]
    public class Ticketing : Migration
    {
        /*
        Manual FILESTREAM setup / verification (run in SSMS, outside a transaction).
        This comment is documentation only; the migration never executes this script.

        1. In SQL Server Configuration Manager > SQL Server Services > instance
           Properties > FILESTREAM, enable Transact-SQL access. Also enable file I/O
           streaming access if the application will use SqlFileStream.
           Apply the settings and restart the SQL Server service as required.

        2. Run the following separately with server configuration permissions.
           Level 1 is sufficient for T-SQL; use level 2 for SqlFileStream / file I/O.
           Preserve a higher existing level; do not downgrade it.

        USE [master];
        GO
        IF EXISTS (SELECT 1 FROM sys.configurations
                   WHERE [name] = N'filestream access level' AND [value] < 1)
        BEGIN
            EXEC sys.sp_configure N'filestream access level', 1;
            RECONFIGURE;
        END;
        SELECT SERVERPROPERTY('FilestreamConfiguredLevel') AS ConfiguredLevel,
               SERVERPROPERTY('FilestreamEffectiveLevel') AS EffectiveLevel;
        GO

        3. Replace YourDatabaseName and the container path below before running.
           UserPool is a SCHEMA inside the application database, not its name.
           The path is on the SQL Server host. Its parent directory must exist;
           the final container directory must NOT exist (SQL Server creates it).
           The SQL Server service account needs access to the parent directory.
           Existing default FILESTREAM storage is reused, including BlobPool storage.

        USE [YourDatabaseName];
        GO
        SET NOCOUNT ON;
        IF @@TRANCOUNT <> 0
            THROW 51010, 'Run FILESTREAM setup outside a transaction.', 1;
        IF DB_NAME() IN (N'master', N'model', N'msdb', N'tempdb')
            THROW 51011, 'Select the application database first.', 1;
        IF COALESCE(CONVERT(int, SERVERPROPERTY('FilestreamEffectiveLevel')), 0) < 1
            THROW 51012, 'Enable FILESTREAM on the instance before continuing.', 1;

        DECLARE @DatabaseName nvarchar(258) = QUOTENAME(DB_NAME());
        DECLARE @FilegroupName sysname;
        DECLARE @ContainerPath nvarchar(260) = N'D:\SqlData\Aban360_Ticketing_Filestream';

        SELECT @FilegroupName = [name]
        FROM sys.filegroups WHERE [type] = 'FD' AND is_default = 1;

        IF @FilegroupName IS NULL
        BEGIN
            SET @FilegroupName = N'UserPoolFileStream';
            IF EXISTS (SELECT 1 FROM sys.filegroups
                       WHERE [name] = @FilegroupName AND [type] <> 'FD')
                THROW 51013, 'UserPoolFileStream exists but is not a FILESTREAM filegroup.', 1;
            IF NOT EXISTS (SELECT 1 FROM sys.filegroups WHERE [name] = @FilegroupName)
                EXEC (N'ALTER DATABASE ' + @DatabaseName
                      + N' ADD FILEGROUP [UserPoolFileStream] CONTAINS FILESTREAM;');
        END;

        IF NOT EXISTS (SELECT 1 FROM sys.database_files AS f
                       JOIN sys.filegroups AS g ON g.data_space_id = f.data_space_id
                       WHERE g.[name] = @FilegroupName AND f.[type] = 2 AND f.[state] = 0)
        BEGIN
            IF EXISTS (SELECT 1 FROM sys.database_files WHERE [name] = N'UserPoolFileStreamData')
                THROW 51014, 'Logical file name is already in use; inspect existing storage.', 1;
            EXEC (N'ALTER DATABASE ' + @DatabaseName
                  + N' ADD FILE (NAME = N''UserPoolFileStreamData'', FILENAME = N'''
                  + REPLACE(@ContainerPath, N'''', N'''''')
                  + N''') TO FILEGROUP ' + QUOTENAME(@FilegroupName) + N';');
        END;

        EXEC (N'ALTER DATABASE ' + @DatabaseName + N' MODIFY FILEGROUP '
              + QUOTENAME(@FilegroupName) + N' DEFAULT;');

        -- Verify: expect a writable default FD group with an ONLINE FILESTREAM file.
        SELECT DB_NAME() AS DatabaseName, g.[name] AS FilegroupName,
               g.is_default, g.is_read_only, f.[name] AS LogicalFileName,
               f.physical_name, f.state_desc
        FROM sys.filegroups AS g
        LEFT JOIN sys.database_files AS f ON f.data_space_id = g.data_space_id
        WHERE g.[type] = 'FD';

        IF NOT EXISTS (SELECT 1 FROM sys.filegroups AS g
                       JOIN sys.database_files AS f ON f.data_space_id = g.data_space_id
                       WHERE g.[type] = 'FD' AND g.is_default = 1
                         AND g.is_read_only = 0 AND f.[type] = 2 AND f.[state] = 0)
            THROW 51015, 'No writable default FILESTREAM filegroup with an online container.', 1;
        PRINT N'FILESTREAM prerequisites are ready. Run the Ticketing migration next.';
        GO

        References:
        https://learn.microsoft.com/en-us/sql/relational-databases/blob/enable-and-configure-filestream
        https://learn.microsoft.com/en-us/sql/relational-databases/blob/create-a-filestream-enabled-database
        */
        public override void Up()
        {
            // FILESTREAM infrastructure is provisioned by the database administrator.
            // Reuse the default FILESTREAM filegroup, as the BlobPool migration does.
            Execute.Sql("""
                IF COALESCE(CONVERT(int, SERVERPROPERTY('FilestreamEffectiveLevel')), 0) = 0
                    THROW 51000, 'Ticketing requires FILESTREAM to be enabled on SQL Server.', 1;

                IF NOT EXISTS (
                    SELECT 1 FROM sys.filegroups AS g
                    JOIN sys.database_files AS f ON f.data_space_id = g.data_space_id
                    WHERE g.[type] = 'FD' AND g.is_default = 1 AND g.is_read_only = 0
                      AND f.[type] = 2 AND f.[state] = 0)
                    THROW 51001, 'Ticketing requires a writable default FILESTREAM filegroup with an online container.', 1;
                """);

            Execute.Sql($"""
                CREATE TABLE [{TableSchema.Name}].[TicketStatus]
                (
                    [Id] SMALLINT NOT NULL CONSTRAINT [PK_TicketStatus] PRIMARY KEY,
                    [Code] NVARCHAR(50) NOT NULL CONSTRAINT [UQ_TicketStatus_Code] UNIQUE,
                    [Title] NVARCHAR(100) NOT NULL
                );

                INSERT INTO [{TableSchema.Name}].[TicketStatus] ([Id], [Code], [Title])
                VALUES (1, N'New', N'جدید'),
                       (2, N'InProgress', N'در حال بررسی'),
                       (3, N'Answered', N'پاسخ داده‌شده'),
                       (4, N'Closed', N'بسته‌شده');

                CREATE TABLE [{TableSchema.Name}].[Ticket]
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
                    CONSTRAINT [FK_Ticket_CreatedByUser] FOREIGN KEY ([CreatedByUserId]) REFERENCES [{TableSchema.Name}].[User] ([Id]),
                    CONSTRAINT [FK_Ticket_AssignedToUser] FOREIGN KEY ([AssignedToUserId]) REFERENCES [{TableSchema.Name}].[User] ([Id]),
                    CONSTRAINT [FK_Ticket_Status] FOREIGN KEY ([StatusId]) REFERENCES [{TableSchema.Name}].[TicketStatus] ([Id])
                );

                CREATE TABLE [{TableSchema.Name}].[TicketMessage]
                (
                    [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_TicketMessage] PRIMARY KEY,
                    [TicketId] BIGINT NOT NULL,
                    [SenderUserId] UNIQUEIDENTIFIER NOT NULL,
                    [Body] NVARCHAR(MAX) NULL,
                    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_TicketMessage_CreatedAt] DEFAULT SYSUTCDATETIME(),
                    CONSTRAINT [FK_TicketMessage_Ticket] FOREIGN KEY ([TicketId]) REFERENCES [{TableSchema.Name}].[Ticket] ([Id]),
                    CONSTRAINT [FK_TicketMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [{TableSchema.Name}].[User] ([Id])
                );

                CREATE TABLE [{TableSchema.Name}].[TicketAttachment]
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
                    CONSTRAINT [FK_TicketAttachment_TicketMessage] FOREIGN KEY ([TicketMessageId]) REFERENCES [{TableSchema.Name}].[TicketMessage] ([Id])
                );

                CREATE TABLE [{TableSchema.Name}].[TicketRating]
                (
                    [Id] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_TicketRating] PRIMARY KEY,
                    [TicketMessageId] BIGINT NOT NULL,
                    [RatedByUserId] UNIQUEIDENTIFIER NOT NULL,
                    [Score] TINYINT NOT NULL,
                    [Comment] NVARCHAR(1023) NULL,
                    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT [DF_TicketRating_CreatedAt] DEFAULT SYSUTCDATETIME(),
                    CONSTRAINT [UQ_TicketRating_TicketMessageId] UNIQUE ([TicketMessageId]),
                    CONSTRAINT [CK_TicketRating_Score] CHECK ([Score] BETWEEN 1 AND 5),
                    CONSTRAINT [FK_TicketRating_TicketMessage] FOREIGN KEY ([TicketMessageId]) REFERENCES [{TableSchema.Name}].[TicketMessage] ([Id]),
                    CONSTRAINT [FK_TicketRating_RatedByUser] FOREIGN KEY ([RatedByUserId]) REFERENCES [{TableSchema.Name}].[User] ([Id])
                );

                CREATE INDEX [IX_Ticket_CreatedByUserId] ON [{TableSchema.Name}].[Ticket] ([CreatedByUserId]);
                CREATE INDEX [IX_Ticket_AssignedToUserId] ON [{TableSchema.Name}].[Ticket] ([AssignedToUserId]);
                CREATE INDEX [IX_Ticket_StatusId_CreatedAt] ON [{TableSchema.Name}].[Ticket] ([StatusId], [CreatedAt]);
                CREATE INDEX [IX_Ticket_CreatedAt] ON [{TableSchema.Name}].[Ticket] ([CreatedAt]);
                CREATE INDEX [IX_TicketMessage_TicketId_CreatedAt] ON [{TableSchema.Name}].[TicketMessage] ([TicketId], [CreatedAt], [Id]);
                CREATE INDEX [IX_TicketMessage_SenderUserId] ON [{TableSchema.Name}].[TicketMessage] ([SenderUserId]);
                CREATE INDEX [IX_TicketAttachment_TicketMessageId] ON [{TableSchema.Name}].[TicketAttachment] ([TicketMessageId]);
                CREATE INDEX [IX_TicketRating_RatedByUserId] ON [{TableSchema.Name}].[TicketRating] ([RatedByUserId]);
                """);
        }

        public override void Down()
        {
            Delete.Table("TicketRating").InSchema(TableSchema.Name);
            Delete.Table("TicketAttachment").InSchema(TableSchema.Name);
            Delete.Table("TicketMessage").InSchema(TableSchema.Name);
            Delete.Table("Ticket").InSchema(TableSchema.Name);
            Delete.Table("TicketStatus").InSchema(TableSchema.Name);
        }
    }
}
