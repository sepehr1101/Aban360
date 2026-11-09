USE [Atlas];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.NowGenerateBillRequest', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NowGenerateBillRequest
    (
        Id bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_NowGenerateBillRequest PRIMARY KEY,
        RequestedAt datetime2(3) NOT NULL,
        RequestDate date NOT NULL,
        MobileNumber nvarchar(max) COLLATE Latin1_General_100_BIN2 NULL,
        BillId nvarchar(max) COLLATE Latin1_General_100_BIN2 NULL,
        CurrentMeterNumber int NOT NULL,
        LimitErrorCode int NOT NULL,
        CONSTRAINT CK_NowGenerateBillRequest_LimitErrorCode CHECK (LimitErrorCode IN (0, 10, 11))
    );

    CREATE INDEX IX_NowGenerateBillRequest_RequestDate
        ON dbo.NowGenerateBillRequest(RequestDate)
        INCLUDE (MobileNumber, BillId);
END;

COMMIT TRANSACTION;
GO
