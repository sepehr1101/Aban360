-- Run manually on the logging database during a maintenance window.
-- Check existing indexes first; equivalent indexes may already exist under other names.
-- Keep large Message/Exception/Properties values out of these indexes.
USE [Aban360];
GO
SET LOCK_TIMEOUT 5000;
IF OBJECT_ID(N'dbo.Logs', N'U') IS NULL
    THROW 50001, 'dbo.Logs does not exist in the selected database.', 1;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes i
    JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
    WHERE i.object_id = OBJECT_ID(N'dbo.Logs') AND i.type IN (1, 2)
      AND i.is_disabled = 0 AND i.is_hypothetical = 0 AND i.has_filter = 0
      AND ic.key_ordinal = 1 AND c.name = N'TimeStamp'
)
AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Logs') AND name = N'IX_Logs_TimeStamp')
    CREATE NONCLUSTERED INDEX IX_Logs_TimeStamp ON dbo.Logs ([TimeStamp] DESC);
GO

-- The deployed Serilog table may store Level as nvarchar(max), which cannot
-- be an index key. Cover its residual filter from the timestamp index instead.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Logs') AND name = N'Level' AND max_length = -1)
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes i
        JOIN sys.index_columns k ON i.object_id = k.object_id AND i.index_id = k.index_id AND k.key_ordinal = 1
        JOIN sys.columns kc ON kc.object_id = k.object_id AND kc.column_id = k.column_id
        WHERE i.object_id = OBJECT_ID(N'dbo.Logs') AND i.type IN (1, 2)
          AND i.is_disabled = 0 AND i.is_hypothetical = 0 AND i.has_filter = 0
          AND kc.name = N'TimeStamp'
          AND EXISTS (
              SELECT 1 FROM sys.index_columns ic
              JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
              WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'Level'
          )
    )
    BEGIN
        IF EXISTS (
            SELECT 1 FROM sys.indexes i
            JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
            JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
            WHERE i.object_id = OBJECT_ID(N'dbo.Logs') AND i.name = N'IX_Logs_TimeStamp'
              AND i.type = 2 AND i.has_filter = 0 AND ic.key_ordinal = 1 AND c.name = N'TimeStamp'
        )
            CREATE NONCLUSTERED INDEX IX_Logs_TimeStamp
                ON dbo.Logs ([TimeStamp] DESC) INCLUDE ([Level]) WITH (DROP_EXISTING = ON);
        ELSE
            CREATE NONCLUSTERED INDEX IX_Logs_TimeStamp_LevelInclude
                ON dbo.Logs ([TimeStamp] DESC) INCLUDE ([Level]);
    END;
    PRINT 'Level is a MAX column: covered by the timestamp index, not used as an index key.';
END
ELSE IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Logs') AND name = N'IX_Logs_Level_TimeStamp')
    CREATE NONCLUSTERED INDEX IX_Logs_Level_TimeStamp ON dbo.Logs ([Level], [TimeStamp] DESC);
GO
