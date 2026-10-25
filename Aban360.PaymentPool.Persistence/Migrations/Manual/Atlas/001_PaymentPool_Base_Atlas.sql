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
    IF SCHEMA_ID(N'PaymentPool') IS NULL EXEC(N'CREATE SCHEMA [PaymentPool]');

    IF OBJECT_ID(N'PaymentPool.Bank', N'U') IS NULL
        CREATE TABLE [PaymentPool].[Bank] (Id smallint IDENTITY(1,1) NOT NULL PRIMARY KEY, BankName nvarchar(255) NOT NULL,
    _3Char nvarchar(3) NULL, Icon nvarchar(max) NULL, CentralBankCode nvarchar(255) NOT NULL, Description nvarchar(max) NULL);

    IF OBJECT_ID(N'PaymentPool.PaymentMethod', N'U') IS NULL
        CREATE TABLE [PaymentPool].[PaymentMethod] (Id smallint NOT NULL PRIMARY KEY, Title nvarchar(255) NOT NULL, Icon nvarchar(max) NULL, Description nvarchar(max) NULL);

    IF OBJECT_ID(N'PaymentPool.CreditorType', N'U') IS NULL
        CREATE TABLE [PaymentPool].[CreditorType] (Id smallint NOT NULL PRIMARY KEY, Title nvarchar(255) NOT NULL, Icon nvarchar(max) NULL);

    IF OBJECT_ID(N'PaymentPool.BankFileStructure', N'U') IS NULL
        CREATE TABLE [PaymentPool].[BankFileStructure] (Id smallint IDENTITY(1,1) NOT NULL PRIMARY KEY, FromIndex smallint NOT NULL, ToIndex smallint NOT NULL,
    StringLenght smallint NOT NULL, Title nvarchar(255) NOT NULL, IsHeader bit NOT NULL, BankStructureItemId smallint NOT NULL);

    IF OBJECT_ID(N'PaymentPool.AccountType', N'U') IS NULL
        CREATE TABLE [PaymentPool].[AccountType] (Id smallint NOT NULL PRIMARY KEY, Title nvarchar(255) NOT NULL);

    IF OBJECT_ID(N'PaymentPool.BankAccount', N'U') IS NULL
        CREATE TABLE [PaymentPool].[BankAccount] (Id smallint NOT NULL PRIMARY KEY, BankId smallint NOT NULL REFERENCES PaymentPool.Bank(Id),
    Title nvarchar(255) NOT NULL, AccountTypeId smallint NOT NULL, RegionId int NOT NULL, RegionTitle nvarchar(255) NOT NULL,
    IBan nvarchar(26) NOT NULL, Number nvarchar(24) NOT NULL, Icon nvarchar(max) NULL);

    IF OBJECT_ID(N'PaymentPool.Uploader', N'U') IS NULL
        CREATE TABLE [PaymentPool].[Uploader] (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, UserId uniqueidentifier NOT NULL, Username nvarchar(255) NOT NULL,
    BankId smallint NOT NULL REFERENCES PaymentPool.Bank(Id), InsertDateTime datetime NOT NULL, InsertRecordCount int NOT NULL,
    Amount bigint NOT NULL, ReferenceNumber nvarchar(20) NULL, DocumentId uniqueidentifier NULL);

    IF OBJECT_ID(N'PaymentPool.Credit', N'U') IS NULL
        CREATE TABLE [PaymentPool].[Credit] (Id bigint IDENTITY(1,1) NOT NULL PRIMARY KEY, BillId nvarchar(15) NOT NULL, PaymentId nvarchar(20) NOT NULL,
    InvoiceId bigint NOT NULL, InvoiceInstallmentId bigint NOT NULL, Amount bigint NOT NULL,
    UploaderId int NOT NULL REFERENCES PaymentPool.Uploader(Id),
    CreditorTypeId smallint NOT NULL REFERENCES PaymentPool.CreditorType(Id),
    PaymentMethodId smallint NOT NULL REFERENCES PaymentPool.PaymentMethod(Id),
    ValidFrom datetime2 NOT NULL, ValidTo datetime2 NULL, InsertLogInfo nvarchar(max) NOT NULL,
    RemoveLogInfo nvarchar(max) NULL, Hash nvarchar(max) NOT NULL);

-- Seed only missing lookup records; existing IDs and values are not overwritten.
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک مرکزی جمهوری اسلامی ایران', N'', N'', N'10', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'10');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک صنعت و معدن', N'SAM', N'ibl32 ibl-bim', N'11', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'11');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک ملت', N'MLT', N'ibl32 ibl-mellat', N'12', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'12');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک رفاه', N'REF', N'ibl32 ibl-rb', N'13', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'13');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک مسکن', N'MSK', N'ibl32 ibl-maskan', N'14', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'14');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک سپه', N'SEP', N'ibl32 ibl-sepah', N'15', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'15');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک کشاورزی', N'KSH', N'ibl32 ibl-bki', N'16', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'16');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک ملی ایران', N'MLI', N'ibl32 ibl-bmi', N'17', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'17');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک تجارت', N'TEJ', N'ibl32 ibl-tejarat', N'18', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'18');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک صادرات ایران', N'SDT', N'ibl32 ibl-bsi', N'19', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'19');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک توسعه صادرات', N'TOS', N'ibl32 ibl-edbi', N'20', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'20');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'پست بانک ایران', N'PST', N'ibl32 ibl-post', N'21', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'21');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک توسعه تعاون', N'', N'ibl32 ibl-tt', N'22', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'22');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'موسسه اعتباری توسعه', N'', N'', N'51', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'51');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک کارآفرین', N'KAR', N'ibl32 ibl-kar', N'53', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'53');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک پارسیان', N'PAR', N'ibl32 ibl-parsian', N'54', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'54');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک اقتصاد نوین', N'EQN', N'ibl32 ibl-en', N'55', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'55');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک سامان', N'SMN', N'ibl32 ibl-sb', N'56', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'56');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک پاسارگاد', N'PSG', N'ibl32 ibl-bpi', N'57', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'57');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک سرمایه', N'SRM', N'ibl32 ibl-sarmayeh', N'58', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'58');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک سینا', N'', N'ibl32 ibl-sina', N'59', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'59');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'قرض الحسنه مهر', N'', N'ibl32 ibl-miran', N'60', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'60');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک شهر', N'', N'ibl32 ibl-shahr', N'61', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'61');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک تات', N'', N'', N'62', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'62');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک انصار', N'', N'ibl32 ibl-ansar', N'63', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'63');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک گردشگری', N'', N'ibl32 ibl-tourism', N'64', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'64');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک حکمت ایرانیان', N'', N'ibl32 ibl-hi', N'65', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'65');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک دی', N'', N'ibl32 ibl-day', N'66', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'66');
INSERT [PaymentPool].[Bank] ([BankName], [_3Char], [Icon], [CentralBankCode], [Description]) SELECT N'بانک ایران زمین', N'', N'ibl32 ibl-iz', N'69', N''
WHERE NOT EXISTS (SELECT 1 FROM PaymentPool.Bank WHERE CentralBankCode=N'69');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=2) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (2,N'خودپرداز');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=3) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (3,N'حضور در شعبه');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=5) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (5,N'اینترانت');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=6) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (6,N'پیامک');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=7) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (7,N'تلفن بانک');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=8) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (8,N'خود دریافت');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=9) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (9,N'بانک دریافت');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=13) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (13,N'وب کیوسک');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=14) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (14,N'پایانه فروش');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.PaymentMethod WHERE Id=59) INSERT PaymentPool.PaymentMethod(Id,Title) VALUES (59,N'اینترنت');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=1) INSERT PaymentPool.CreditorType(Id,Title) VALUES (1,N'پرداخت الکترونیک');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=2) INSERT PaymentPool.CreditorType(Id,Title) VALUES (2,N'ثبت دستی وصولی');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=3) INSERT PaymentPool.CreditorType(Id,Title) VALUES (3,N'تهاتر');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=4) INSERT PaymentPool.CreditorType(Id,Title) VALUES (4,N'چک');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=5) INSERT PaymentPool.CreditorType(Id,Title) VALUES (5,N'دفترچه قسط');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.CreditorType WHERE Id=6) INSERT PaymentPool.CreditorType(Id,Title) VALUES (6,N'سفته');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.AccountType WHERE Id=1) INSERT PaymentPool.AccountType(Id,Title) VALUES (1,N'آب بها');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.AccountType WHERE Id=2) INSERT PaymentPool.AccountType(Id,Title) VALUES (2,N'حق انشعاب');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.AccountType WHERE Id=3) INSERT PaymentPool.AccountType(Id,Title) VALUES (3,N'تبصره');
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=1) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (0,1,1,N'کد شرکت خدماتی',1,1);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=2) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (1,4,3,N'کد شرکت تابعه',1,2);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=3) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (4,6,2,N'کد بانک',1,3);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=4) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (6,14,8,N'تاریخ ارسال فایل',1,4);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=5) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (14,24,10,N'جمع مبلغ',1,5);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=1 AND BankStructureItemId=6) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (24,32,8,N'تعداد سطرها',1,6);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=7) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (0,6,6,N'کد شعبه',0,7);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=8) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (6,8,2,N'روش پرداخت',0,8);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=9) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (8,16,8,N'تاریخ پرداخت',0,9);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=10) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (16,29,13,N'شناسه قبض',0,10);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=11) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (29,42,13,N'شناسه پرداخت',0,11);
IF NOT EXISTS (SELECT 1 FROM PaymentPool.BankFileStructure WHERE IsHeader=0 AND BankStructureItemId=12) INSERT PaymentPool.BankFileStructure(FromIndex,ToIndex,StringLenght,Title,IsHeader,BankStructureItemId) VALUES (42,48,6,N'کد پیگیری',0,12);
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
