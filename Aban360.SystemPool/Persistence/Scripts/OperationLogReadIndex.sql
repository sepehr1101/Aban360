-- Run manually on the report server. This creates an index, not an API migration.
USE [CustomerWarehouse];
GO
SET LOCK_TIMEOUT 5000;
IF OBJECT_ID(N'dbo.OpLog', N'U') IS NULL
    THROW 50001, 'dbo.OpLog does not exist in the selected database.', 1;

-- An existing unfiltered index with this leading key can already serve the range.
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes i
    JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
    WHERE i.object_id = OBJECT_ID(N'dbo.OpLog')
      AND i.type IN (1, 2) AND i.is_disabled = 0 AND i.is_hypothetical = 0
      AND i.has_filter = 0 AND ic.key_ordinal = 1 AND c.name = N'RegisterDateTime'
)
    CREATE NONCLUSTERED INDEX IX_OpLog_RegisterDateTime
        ON dbo.OpLog (RegisterDateTime DESC, Id DESC);
GO
