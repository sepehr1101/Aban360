using FluentMigrator;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Persistence.Migrations.Enums;
using System.Reflection;
using Aban360.UserPool.Persistence.Constants;

namespace Aban360.UserPool.Persistence.Migrations
{
    [Migration(1403082101)]
    public class DbInitialDesign : Migration
    {
        string _schema=TableSchema.Name, Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _10 = 10;
        public override void Up()
        {
            Create.Schema(_schema);

            var methods =
               GetType()
              .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(m => m.Name.StartsWith("Create"))
              .ToList();
            methods.ForEach(m => m.Invoke(this, null));
        }
        public override void Down()
        {
            var tableNames =
              GetType()
             .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
             .Where(m => m.Name.StartsWith("Create"))
             .Select(m=>m.Name.Replace("Create",string.Empty))
             .ToList();
            tableNames.ForEach(t => Delete.Table(t));
        }

        private void CreateCaptchaDisplayMode()
        {
            var table = TableName.CaptchaDisplayMode;
            Create.Table(nameof(TableName.CaptchaDisplayMode)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("DisplayModeEnumId").AsInt16().NotNullable()
                .WithColumn("Name").AsAnsiString(_31).NotNullable()
                .WithColumn("Title").AsString(_31).NotNullable();
        }
        private void CreateCapchaLanguage()
        {
            var table= TableName.CaptchaLanguage;
            Create.Table(nameof(TableName.CaptchaLanguage)).InSchema(_schema)   
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("LanguageId").AsInt16().NotNullable()
                .WithColumn("Name").AsAnsiString(_31).NotNullable() 
                .WithColumn("Title").AsString(_31).NotNullable();
        }
        private void CreateCaptcha()
        {   
            var table = TableName.Captcha;
            Create.Table(nameof(TableName.Captcha)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.CaptchaLanguage}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.CaptchaLanguage, table), _schema, nameof(TableName.CaptchaLanguage), Id)
                .WithColumn($"{TableName.CaptchaDisplayMode}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.CaptchaDisplayMode, table), _schema, nameof(TableName.CaptchaDisplayMode), Id)
                .WithColumn("ShowThousandSeperator").AsBoolean().NotNullable()
                //.WithColumn("ShowRefreshButton").AsBoolean().NotNullable()
                //.WithColumn("RefreshButtonClass").AsAnsiString().NotNullable()
                .WithColumn("FontName").AsAnsiString(_255).NotNullable()
                .WithColumn("FontSize").AsInt32().NotNullable()
                .WithColumn("ForeColor").AsAnsiString(_255).NotNullable()
                .WithColumn("BackColor").AsAnsiString(_255).NotNullable()
                .WithColumn("ExpiresAfter").AsInt32().NotNullable()
                //.WithColumn("ValidationMessage").AsString(_1023).NotNullable()
                //.WithColumn("ValidationMessageClass").AsAnsiString(_255).NotNullable()
                .WithColumn("RateLimit").AsInt32().NotNullable()
                //.WithColumn("RateLimitMessage").AsString(_1023).NotNullable()               
                .WithColumn("Noise").AsAnsiString(_255).NotNullable()
                .WithColumn("EncryptionKey").AsString(_1023).NotNullable()
                .WithColumn("NonceKey").AsString(_1023).NotNullable()
                .WithColumn("Direction").AsAnsiString(3).NotNullable()
                .WithColumn("Min").AsInt32().NotNullable()
                .WithColumn("Max").AsInt32().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("IsSelected").AsBoolean().NotNullable();
                //.WithColumn("InputPlaceholder").AsString(_255).NotNullable()
                //.WithColumn("HiddenInputName").AsAnsiString(_255).NotNullable()
                //.WithColumn("HiddenTokenName").AsAnsiString(_255).NotNullable()
                //.WithColumn("InputName").AsAnsiString(_255).NotNullable()
                //.WithColumn("InputClass").AsAnsiString(_255).NotNullable()
                //.WithColumn("InputTemplate").AsString(int.MaxValue)
                //.WithColumn("Identifier").AsAnsiString(_255).NotNullable();
        }

        private void CreateOperationType()
        {
            Create.Table(nameof(TableName.OperationType)).InSchema(_schema)
               .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(TableName.OperationType))
               .WithColumn("Title").AsString(_255)
               .WithColumn("Css").AsString(_1023);
        }
        private void CreateDeepLog()
        {
            Create.Table(nameof(TableName.DeepLog)).InSchema(_schema)
               .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(TableName.DeepLog)).Identity()
               .WithColumn($"{nameof(TableName.OperationType)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OperationType, TableName.DeepLog), _schema, nameof(TableName.OperationType), Id)
               .WithColumn("PrimaryDb").AsString(_255).NotNullable()
               .WithColumn("PrimaryTable").AsString(_255).NotNullable()
               .WithColumn("PrimaryId").AsString(63).NotNullable()
               .WithColumn("ValueBefore").AsString(int.MaxValue).Nullable()
               .WithColumn("ValueAfter").AsString(int.MaxValue).Nullable()
               .WithColumn("Ip").AsAnsiString(64).NotNullable()
               .WithColumn("InsertDateTime").AsDateTime().NotNullable()
               .WithColumn("UserId").AsGuid().NotNullable()
               .WithColumn("ClientInfo").AsString(int.MaxValue).NotNullable();
        }

        private void CreateUser()
        {
            var table = TableName.User;
            Create.Table(nameof(TableName.User)).InSchema(_schema)
                .WithColumn(Id).AsGuid().PrimaryKey(NamingHelper.Pk(table))               
                .WithColumn("FullName").AsString(_255).NotNullable()
                .WithColumn("DisplayName").AsString(_255).NotNullable()
                .WithColumn("Username").AsString(_255).Unique(NamingHelper.Uq(table,"Username")).NotNullable()
                .WithColumn("Password").AsString(int.MaxValue).NotNullable()
                .WithColumn("Mobile").AsAnsiString(11).NotNullable()
                .WithColumn("MobileConfirmed").AsBoolean().NotNullable()    
                .WithColumn("HasTwoStepVerification").AsBoolean().NotNullable()
                .WithColumn("InvalidLoginAttemptCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("SerialNumber").AsAnsiString(36).Nullable()
                .WithColumn("LatestLoginDateTime").AsDateTime().Nullable()
                .WithColumn("LockTimespan").AsDateTime().Nullable()
                .WithColumn("PreviousId").AsGuid().Nullable()
                     .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameof(TableName.User), Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateRole()
        {
            var table = TableName.Role;
            Create.Table(nameof(TableName.Role)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Name").AsAnsiString(_255).NotNullable() 
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("DefaultClaims").AsString(int.MaxValue).Nullable()
                .WithColumn("SensitiveInfo").AsBoolean().NotNullable()
                .WithColumn("IsRemovable").AsBoolean().NotNullable()
                .WithColumn("PreviousId").AsInt32().Nullable()
                    .ForeignKey(NamingHelper.Fk(table, table, "PreviousId"), _schema, nameof(TableName.Role), Id)
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateUserRole()
        {
            var table = TableName.UserRole;
            Create.Table(nameof(TableName.UserRole)).InSchema(_schema)  
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), _schema, nameof(TableName.User), Id)
                .WithColumn($"{nameof(TableName.Role)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Role, TableName.UserRole), _schema, nameof(TableName.Role), Id)
                .WithColumn("InsertGroupId").AsGuid().NotNullable()
                .WithColumn("RemoveGroupId").AsGuid().Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateUserClaim()
        {
            var table = TableName.UserClaim;
            Create.Table(nameof(TableName.UserClaim)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), _schema, nameof(TableName.User), Id)
                .WithColumn("ClaimTypeId").AsInt16().NotNullable()
                .WithColumn("ClaimValue").AsString(_1023).NotNullable()
                .WithColumn("InsertGroupId").AsGuid().NotNullable()
                .WithColumn("RemoveGroupId").AsGuid().Nullable()
                .WithColumn("ValidFrom").AsDateTime2().NotNullable()
                .WithColumn("ValidTo").AsDateTime2().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateUserToken()
        {
            var table = TableName.UserToken;
            Create.Table(nameof(TableName.UserToken)).InSchema(_schema)
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), _schema, nameof(TableName.User), Id)
                .WithColumn("AccessTokenExpiresDateTime").AsDateTime().NotNullable()
                .WithColumn("AccessTokenHash").AsString(_1023).NotNullable()
                .WithColumn("RefreshTokenExpiresDateTime").AsDateTime().NotNullable()
                .WithColumn("RefreshTokenIdHash").AsString(_1023).NotNullable()
                .WithColumn("RefreshTokenIdHashSource").AsString(_1023).Nullable();
        }

        private void CreateInvalidLoginReason()
        {
            var table = TableName.InvalidLoginReason;
            Create.Table(nameof(TableName.InvalidLoginReason)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateLogoutReason()
        {
            var table = TableName.LogoutReason;
            Create.Table(nameof(TableName.LogoutReason)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void CreateUserLogin()
        {
            var table = TableName.UserLogin;
            Create.Table(nameof(TableName.UserLogin)).InSchema(_schema)
                .WithColumn(Id).AsGuid().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Username").AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), _schema, nameof(TableName.User), Id)
                .WithColumn("FirstStepDateTime").AsDateTime().NotNullable()
                .WithColumn("Ip").AsAnsiString(15).NotNullable()
                .WithColumn("FirstStepSuccess").AsBoolean().NotNullable()
                .WithColumn($"{nameof(TableName.InvalidLoginReason)}{Id}").AsInt16().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.InvalidLoginReason, table), _schema, nameof(TableName.InvalidLoginReason), Id)
                .WithColumn("WrongPassword").AsString(_1023).Nullable()
                .WithColumn("AppVersion").AsString(15).NotNullable()
                .WithColumn("TwoStepCode").AsAnsiString(15).Nullable()
                .WithColumn("TwoStepExpireDateTime").AsDateTime().Nullable()
                .WithColumn("TwoStepInsertDateTime").AsDateTime().Nullable()
                .WithColumn("TwoStepWasSuccessful").AsBoolean().Nullable()
                .WithColumn("PreviousFailureIsShown").AsBoolean().NotNullable()
                .WithColumn("LogoutDateTime").AsDateTime().Nullable()
                .WithColumn($"{nameof(TableName.LogoutReason)}{Id}").AsInt16().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.LogoutReason, table), _schema, nameof(TableName.LogoutReason), Id)
                .WithColumn("LogInfo").AsAnsiString(int.MaxValue).NotNullable();
        }
               
        private void CreateApp()
        {
            var table = TableName.App;
            Create.Table(nameof(TableName.App)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Style").AsString(_1023).NotNullable()
                .WithColumn("InMenu").AsBoolean().NotNullable()
                .WithColumn("LogicalOrder").AsInt32().NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();
        }
        private void CreateModule()
        {
            var table = TableName.Module;
            Create.Table(nameof(TableName.Module)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.App)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.App, table), _schema, nameof(TableName.App), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Style").AsString(_1023).Nullable()
                .WithColumn("InMenu").AsBoolean().NotNullable()
                .WithColumn("LogicalOrder").AsInt32().NotNullable()
                .WithColumn("ClientRoute").AsString(_255).Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();
        }
        private void CreateSubModule()
        {
            var table = TableName.SubModule;
            Create.Table(nameof(TableName.SubModule)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.Module)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Module, table), _schema, nameof(TableName.Module), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Style").AsString(_1023).Nullable()
                .WithColumn("InMenu").AsBoolean().NotNullable()
                .WithColumn("LogicalOrder").AsInt32().NotNullable()
                .WithColumn("ClientRoute").AsString(_255).Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();
        }
        private void CreateEndpoint()
        {
            var table = TableName.Endpoint;
            Create.Table(nameof(TableName.Endpoint)).InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.SubModule)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.SubModule, table), _schema, nameof(TableName.SubModule), Id)
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Style").AsString(_1023).Nullable()
                .WithColumn("InMenu").AsBoolean().NotNullable()
                .WithColumn("LogicalOrder").AsInt32().NotNullable()
                .WithColumn("AuthValue").AsString(_255).Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable();
        }

        private void CreateTokenFailureType()
        {
            var table = TableName.TokenFailureType;
            Create.Table(nameof(TableName.TokenFailureType)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255).NotNullable();
        }

        private void CreateUsageLevel1()
        {
            var table= TableName.UsageLevel1;
            Create.Table(nameof(TableName.UsageLevel1)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        
        private void CreateUsageLevel2()
        {
            var table= TableName.UsageLevel2;
            Create.Table(nameof(TableName.UsageLevel2)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel1}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel1, table), _schema, nameof(TableName.UsageLevel1),Id);
        }

        private void CreateUsageLevel3()
        {
            var table= TableName.UsageLevel3;
            Create.Table(nameof(TableName.UsageLevel3)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel2}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel2, table), _schema, nameof(TableName.UsageLevel2), Id);
        }
        
        private void CreateUsageLevel4()
        {
            var table= TableName.UsageLevel4;
            Create.Table(nameof(TableName.UsageLevel4)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn($"{TableName.UsageLevel3}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.UsageLevel3, table), _schema, nameof(TableName.UsageLevel3), Id);
        }

        private void CreateUserWorkday()
        {
            var table=TableName.UserWorkday;
            Create.Table(nameof(TableName.UserWorkday)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UserFullname").AsString(_255).NotNullable()
                .WithColumn("FromReadingNumber").AsAnsiString(_31).NotNullable()
                .WithColumn("ToReadingNumber").AsAnsiString(_31).NotNullable()
                .WithColumn("DateJalali").AsString(_10).NotNullable()
                .WithColumn("ZoneId").AsInt32().NotNullable()
                .WithColumn("ZoneTitle").AsString(_255).NotNullable();
        }

        private void CreateUserLeave()
        {
            var table=TableName.UserLeave;
            Create.Table(nameof(TableName.UserLeave)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("RegiatereId").AsInt16().NotNullable()
                .WithColumn("RegiatereFullname").AsString(_255).NotNullable()
                .WithColumn("RegiatereDatetime").AsDateTime().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("UserFullname").AsString(_255).NotNullable()
                .WithColumn("FromDateJalali").AsString(_10).NotNullable()
                .WithColumn("ToDateJalali").AsString(_10).NotNullable();
        }

        private void CreateOfficialHoliday()
        {
            var table=TableName.OfficialHoliday;
            Create.Table(nameof(TableName.OfficialHoliday)).InSchema(_schema)
                .WithColumn("Id").AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity().NotNullable()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("DateJalali").AsString(_10).NotNullable();
        }

    }
}