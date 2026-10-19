-- Run manually on the logging database during a maintenance window.
-- Check existing indexes first; equivalent indexes may already exist under other names.
-- Keep large Message/Exception/Properties values out of these indexes.
USE [Aban360];
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Logs') AND name = N'IX_Logs_TimeStamp')
    CREATE NONCLUSTERED INDEX IX_Logs_TimeStamp ON dbo.Logs ([TimeStamp] DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Logs') AND name = N'IX_Logs_Level_TimeStamp')
    CREATE NONCLUSTERED INDEX IX_Logs_Level_TimeStamp ON dbo.Logs ([Level], [TimeStamp] DESC);
GO
