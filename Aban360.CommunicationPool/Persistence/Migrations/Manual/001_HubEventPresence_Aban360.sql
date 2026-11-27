USE [Aban360];
GO
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
BEGIN TRANSACTION;

IF COL_LENGTH(N'CommunicationPool.HubEvent', N'LastSeenDateTime') IS NULL
    ALTER TABLE CommunicationPool.HubEvent ADD LastSeenDateTime datetime2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'CommunicationPool.HubEvent')
                 AND name = N'IX_HubEvent_OnlinePresence')
    EXEC(N'CREATE INDEX IX_HubEvent_OnlinePresence
           ON CommunicationPool.HubEvent(LastSeenDateTime)
           INCLUDE (UserId, ConnectionId, ConnectDateTime, DisconnectDateTime)
           WHERE DisconnectDateTime IS NULL');

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'CommunicationPool.HubEvent')
                 AND name = N'IX_HubEvent_ConnectionId')
    CREATE INDEX IX_HubEvent_ConnectionId ON CommunicationPool.HubEvent(ConnectionId);

COMMIT TRANSACTION;
GO
