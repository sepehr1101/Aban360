USE [Atlas];
GO

-- Run while return-bill writes are stopped, before enabling the new allocator.
SET XACT_ABORT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.ReturnBillMinutesCounter', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.ReturnBillMinutesCounter
        (
            Id tinyint NOT NULL,
            LastNumber int NOT NULL,
            UpdatedAt datetime2(3) NOT NULL
                CONSTRAINT DF_ReturnBillMinutesCounter_UpdatedAt DEFAULT SYSUTCDATETIME(),
            CONSTRAINT PK_ReturnBillMinutesCounter PRIMARY KEY (Id),
            CONSTRAINT CK_ReturnBillMinutesCounter_SingleRow CHECK (Id = 1),
            CONSTRAINT CK_ReturnBillMinutesCounter_LastNumber CHECK (LastNumber >= 0)
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.ReturnBillMinutesCounter WITH (UPDLOCK, HOLDLOCK)
        WHERE Id = 1
    )
    BEGIN
        DECLARE @LastNumber bigint;
        SELECT @LastNumber = ISNULL(MAX(N.Number), 0)
        FROM
        (
            SELECT TRY_CONVERT(bigint, jalase_no) AS Number FROM dbo.autoback
            UNION ALL
            SELECT TRY_CONVERT(bigint, jalase_no) FROM dbo.repair
            UNION ALL
            SELECT CONVERT(bigint, 0)
        ) AS N;

        IF @LastNumber >= 2147483647
            THROW 50001, N'شماره جلسه از ظرفیت نوع int عبور کرده است.', 1;

        INSERT INTO dbo.ReturnBillMinutesCounter (Id, LastNumber)
        VALUES (1, CONVERT(int, @LastNumber));
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
