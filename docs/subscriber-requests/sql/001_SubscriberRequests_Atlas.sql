-- Select Atlas on the intended SQL Server before executing. No migration runner is used.
SET NOCOUNT ON;
SET XACT_ABORT ON;
IF DB_NAME() <> N'Atlas'
    THROW 51060, N'این اسکریپت فقط در دیتابیس Atlas قابل اجرا است.', 1;
IF (SELECT compatibility_level FROM sys.databases WHERE database_id=DB_ID()) < 130
    THROW 51061, N'حداقل compatibility level مورد نیاز 130 است.', 1;

BEGIN TRY
    BEGIN TRANSACTION;
    IF SCHEMA_ID(N'SubscriberRequest') IS NULL EXEC(N'CREATE SCHEMA SubscriberRequest');

    IF OBJECT_ID(N'SubscriberRequest.Definition', N'U') IS NULL
        CREATE TABLE SubscriberRequest.Definition (
            RequestCode bigint NOT NULL,
            Version int NOT NULL,
            Title nvarchar(200) NOT NULL,
            HandlerName nvarchar(200) NOT NULL,
            PayloadSchema nvarchar(max) NOT NULL,
            IsEnabled bit NOT NULL CONSTRAINT DF_SubscriberRequestDefinition_Enabled DEFAULT(1),
            CONSTRAINT PK_SubscriberRequestDefinition PRIMARY KEY(RequestCode,Version),
            CONSTRAINT CK_SubscriberRequestDefinition_Version CHECK(Version>0),
            CONSTRAINT CK_SubscriberRequestDefinition_Schema CHECK(ISJSON(PayloadSchema)=1)
        );

    IF OBJECT_ID(N'SubscriberRequest.Draft', N'U') IS NULL
        CREATE TABLE SubscriberRequest.Draft (
            Id uniqueidentifier NOT NULL CONSTRAINT PK_SubscriberRequestDraft PRIMARY KEY,
            TrackingCode varchar(32) NOT NULL CONSTRAINT UQ_SubscriberRequestDraft_Tracking UNIQUE,
            OwnerId uniqueidentifier NOT NULL,
            BillId varchar(20) NOT NULL,
            Mobile varchar(11) NOT NULL,
            RequestCode bigint NOT NULL,
            DefinitionVersion int NOT NULL,
            HandlerName nvarchar(200) NOT NULL,
            PayloadSchema nvarchar(max) NOT NULL,
            Payload nvarchar(max) NOT NULL,
            Status int NOT NULL,
            CreatedAt datetime2(7) NOT NULL,
            ConfirmedAt datetime2(7) NULL,
            CompletedAt datetime2(7) NULL,
            FailedAttempts int NOT NULL CONSTRAINT DF_SubscriberRequestDraft_Attempts DEFAULT(0),
            ResultJson nvarchar(max) NULL,
            CONSTRAINT FK_SubscriberRequestDraft_Definition FOREIGN KEY(RequestCode,DefinitionVersion)
                REFERENCES SubscriberRequest.Definition(RequestCode,Version),
            CONSTRAINT CK_SubscriberRequestDraft_Payload CHECK(ISJSON(Payload)=1 AND ISJSON(PayloadSchema)=1),
            CONSTRAINT CK_SubscriberRequestDraft_Result CHECK(ResultJson IS NULL OR ISJSON(ResultJson)=1),
            CONSTRAINT CK_SubscriberRequestDraft_Status CHECK(Status IN(0,1,2,3)),
            CONSTRAINT CK_SubscriberRequestDraft_Attempts CHECK(FailedAttempts BETWEEN 0 AND 10),
            CONSTRAINT CK_SubscriberRequestDraft_State CHECK(
                (Status IN(0,3) AND ConfirmedAt IS NULL AND CompletedAt IS NULL) OR
                (Status=1 AND ConfirmedAt IS NOT NULL AND CompletedAt IS NULL) OR
                (Status=2 AND ConfirmedAt IS NOT NULL AND CompletedAt IS NOT NULL))
        );

    IF OBJECT_ID(N'SubscriberRequest.Challenge', N'U') IS NULL
        CREATE TABLE SubscriberRequest.Challenge (
            Sequence bigint IDENTITY(1,1) NOT NULL CONSTRAINT UQ_SubscriberRequestChallenge_Sequence UNIQUE,
            Id uniqueidentifier NOT NULL CONSTRAINT PK_SubscriberRequestChallenge PRIMARY KEY,
            RequestId uniqueidentifier NOT NULL REFERENCES SubscriberRequest.Draft(Id),
            CodeHash varbinary(32) NOT NULL,
            CreatedAt datetime2(7) NOT NULL,
            SentAt datetime2(7) NULL,
            ExpiresAt datetime2(7) NULL,
            ConsumedAt datetime2(7) NULL,
            DeliveryFailed bit NOT NULL CONSTRAINT DF_SubscriberRequestChallenge_Failed DEFAULT(0),
            CONSTRAINT CK_SubscriberRequestChallenge_Hash CHECK(DATALENGTH(CodeHash)=32),
            CONSTRAINT CK_SubscriberRequestChallenge_Lifetime CHECK(
                (SentAt IS NULL AND ExpiresAt IS NULL AND ConsumedAt IS NULL) OR
                (SentAt IS NOT NULL AND ExpiresAt IS NOT NULL AND ExpiresAt>SentAt AND
                 ExpiresAt<=DATEADD(SECOND,120,SentAt) AND
                 (ConsumedAt IS NULL OR (ConsumedAt>=SentAt AND ConsumedAt<ExpiresAt)))),
            CONSTRAINT CK_SubscriberRequestChallenge_Delivery CHECK(DeliveryFailed=0 OR SentAt IS NULL)
        );

    IF OBJECT_ID(N'SubscriberRequest.Submission', N'U') IS NULL
        CREATE TABLE SubscriberRequest.Submission (
            RequestId uniqueidentifier NOT NULL CONSTRAINT PK_SubscriberRequestSubmission PRIMARY KEY
                REFERENCES SubscriberRequest.Draft(Id),
            RequestCode bigint NOT NULL,
            OwnerId uniqueidentifier NOT NULL,
            BillId varchar(20) NOT NULL,
            Payload nvarchar(max) NOT NULL,
            RegisteredAt datetime2(7) NOT NULL,
            CONSTRAINT CK_SubscriberRequestSubmission_Payload CHECK(ISJSON(Payload)=1)
        );

    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'SubscriberRequest.Draft') AND name=N'IX_SubscriberRequestDraft_Owner')
        CREATE INDEX IX_SubscriberRequestDraft_Owner ON SubscriberRequest.Draft(OwnerId,CreatedAt) INCLUDE(Mobile);
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'SubscriberRequest.Draft') AND name=N'IX_SubscriberRequestDraft_Mobile')
        CREATE INDEX IX_SubscriberRequestDraft_Mobile ON SubscriberRequest.Draft(Mobile);
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'SubscriberRequest.Challenge') AND name=N'IX_SubscriberRequestChallenge_Request')
        CREATE INDEX IX_SubscriberRequestChallenge_Request ON SubscriberRequest.Challenge(RequestId,Sequence DESC);
    IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'SubscriberRequest.Challenge') AND name=N'IX_SubscriberRequestChallenge_Created')
        CREATE INDEX IX_SubscriberRequestChallenge_Created ON SubscriberRequest.Challenge(CreatedAt) INCLUDE(RequestId);

    -- Service names/codes are from the CRM menu, inspected 2026-11-03.
    -- Schemas below define this API's initial contracts, not an export of the CRM forms.
    -- All services initially register a submission for review. Specialized handlers can replace this per definition version.
    DECLARE @definitions TABLE(RequestCode bigint,Title nvarchar(200),Properties nvarchar(max),Required nvarchar(max));
    INSERT @definitions VALUES
    (15033585000,N'اعلام کارکرد کنتور و صدور قبض آنی',N'{"reading":{"type":"integer","minimum":0}}',N'["reading"]'),
    (10031452100,N'تغییر و ثبت شماره همراه',N'{"newMobile":{"type":"string","pattern":"^09[0-9]{9}$"}}',N'["newMobile"]'),
    (15033584000,N'درخواست بررسی صورتحساب',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (10000000014,N'اعلام اشتباه در قرائت',N'{"reading":{"type":"integer","minimum":0},"description":{"type":"string","maxLength":2000}}',N'["reading"]'),
    (15033573000,N'درخواست تغییر نام و مشخصات',N'{"firstName":{"type":"string","minLength":1,"maxLength":100},"lastName":{"type":"string","minLength":1,"maxLength":100},"nationalCode":{"type":"string","pattern":"^[0-9]{10}$"}}',N'["firstName","lastName","nationalCode"]'),
    (15033571000,N'درخواست انشعاب فاضلاب',N'{"address":{"type":"string","minLength":1,"maxLength":1000},"postalCode":{"type":"string","pattern":"^[0-9]{10}$"}}',N'["address","postalCode"]'),
    (15033572000,N'درخواست تغییر کاربری انشعاب آب',N'{"usageId":{"type":"integer","minimum":1}}',N'["usageId"]'),
    (15033574000,N'درخواست تغییر تعداد واحد',N'{"domesticUnits":{"type":"integer","minimum":0},"commercialUnits":{"type":"integer","minimum":0},"otherUnits":{"type":"integer","minimum":0}}',N'["domesticUnits","commercialUnits","otherUnits"]'),
    (15033575000,N'درخواست تغییر ظرفیت قراردادی',N'{"capacity":{"type":"number","minimum":0}}',N'["capacity"]'),
    (10000000010,N'درخواست خانوار شماری',N'{"householdCount":{"type":"integer","minimum":1}}',N'["householdCount"]'),
    (10000000011,N'اعلام خروج از ساخت و ساز',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033576000,N'درخواست تغییر قطر انشعاب',N'{"diameterId":{"type":"integer","minimum":1}}',N'["diameterId"]'),
    (15033579000,N'درخواست تفکیک کنتور',N'{"meterCount":{"type":"integer","minimum":2},"description":{"type":"string","maxLength":2000}}',N'["meterCount"]'),
    (15033580000,N'درخواست تغییر مکان کنتور',N'{"newLocation":{"type":"string","minLength":1,"maxLength":1000}}',N'["newLocation"]'),
    (15033581000,N'درخواست آزمایش کنتور',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033582000,N'درخواست تعویض کنتور',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (10000000015,N'اعلام خرابی کنتور',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033594088,N'اطلاع رسانی قطعی آب',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033577000,N'نصب سیفون اضافی فاضلاب',N'{"count":{"type":"integer","minimum":1},"diameterId":{"type":"integer","minimum":1}}',N'["count","diameterId"]'),
    (10000000017,N'درخواست تغییر قطر سیفون',N'{"diameterId":{"type":"integer","minimum":1}}',N'["diameterId"]'),
    (10000000016,N'درخواست تعویض سیفون',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (10000000013,N'درخواست جابجایی سیفون',N'{"newLocation":{"type":"string","minLength":1,"maxLength":1000}}',N'["newLocation"]'),
    (15033586000,N'قطع موقت انشعاب آب',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033587000,N'وصل مجدد انشعاب آب',N'{"description":{"type":"string","minLength":1,"maxLength":2000}}',N'["description"]'),
    (15033578000,N'جمع آوری یا ادغام انشعاب آب',N'{"operation":{"type":"string","enum":["remove","merge"]},"targetBillId":{"type":"string","pattern":"^[0-9]{1,20}$"},"description":{"type":"string","maxLength":2000}}',N'["operation"]'),
    (15033591099,N'پاسخ به استعلام مالکیت محضر',N'{"notaryLetterNumber":{"type":"string","minLength":1,"maxLength":100}}',N'["notaryLetterNumber"]'),
    (15033588000,N'فروش آب تانکری',N'{"volume":{"type":"number","minimum":1},"deliveryAddress":{"type":"string","minLength":1,"maxLength":1000}}',N'["volume","deliveryAddress"]'),
    (15033593000,N'رسیدگی به حوادث',N'{"description":{"type":"string","minLength":1,"maxLength":2000},"address":{"type":"string","minLength":1,"maxLength":1000}}',N'["description","address"]');

    INSERT SubscriberRequest.Definition(RequestCode,Version,Title,HandlerName,PayloadSchema,IsEnabled)
    SELECT d.RequestCode,1,d.Title,N'RegisterSubscriberRequestHandler',
        N'{"type":"object","additionalProperties":false,"properties":'+d.Properties+N',"required":'+d.Required+N'}',1
    FROM @definitions d
    WHERE NOT EXISTS(SELECT 1 FROM SubscriberRequest.Definition e WHERE e.RequestCode=d.RequestCode AND e.Version=1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
