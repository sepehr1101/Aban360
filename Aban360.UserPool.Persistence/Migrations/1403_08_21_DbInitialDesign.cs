using FluentMigrator;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Persistence.Migrations.Enums;
using System.Reflection;

namespace Aban360.UserPool.Persistence.Migrations
{
    [Migration(1403082101)]
    public class DbInitialDesign : Migration
    {
        string Id = nameof(Id), Hash = nameof(Hash);
        int _31=31, _255 = 255, _1023 = 1023;
        public override void Up()
        {
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
            Create.Table(nameof(TableName.CaptchaDisplayMode))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("DisplayModeEnumId").AsInt16().NotNullable()
                .WithColumn("Name").AsAnsiString(_31).NotNullable()
                .WithColumn("Tite").AsString(_31).NotNullable();
        }
        private void CreateCapchaLanguage()
        {
            var table= TableName.CaptchaLanguage;
            Create.Table(nameof(TableName.CaptchaLanguage))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("LanguageId").AsInt16().NotNullable()
                .WithColumn("Name").AsAnsiString(_31).NotNullable() 
                .WithColumn("Title").AsString(_31).NotNullable();
        }
        private void CreateCaptcha()
        {   
            var table = TableName.Captcha;
            Create.Table(nameof(TableName.Captcha))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{TableName.CaptchaLanguage}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.CaptchaLanguage,table),nameof(TableName.CaptchaLanguage),Id)
                .WithColumn($"{TableName.CaptchaDisplayMode}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.CaptchaDisplayMode,table), nameof(TableName.CaptchaDisplayMode), Id)
                .WithColumn("ShowThousandSeperator").AsBoolean().NotNullable()
                .WithColumn("ShowRefreshButton").AsBoolean().NotNullable()
                .WithColumn("RefreshButtonClass").AsAnsiString().NotNullable()
                .WithColumn("FontName").AsAnsiString(_255).NotNullable()
                .WithColumn("FontSize").AsInt32().NotNullable()
                .WithColumn("ForeColor").AsAnsiString(_255).NotNullable()
                .WithColumn("BackColor").AsAnsiString(_255).NotNullable()
                .WithColumn("ExpiresAfter").AsInt32().NotNullable()
                .WithColumn("ValidationMessage").AsString(_1023).NotNullable()
                .WithColumn("ValidationMessageClass").AsAnsiString(_255).NotNullable()
                .WithColumn("RateLimit").AsInt32().NotNullable()
                .WithColumn("RateLimitMessage").AsString(_1023).NotNullable()               
                .WithColumn("Noise").AsAnsiString(_255).NotNullable()
                .WithColumn("EncryptionKey").AsString(_1023).NotNullable()
                .WithColumn("NonceKey").AsString(_1023).NotNullable()
                .WithColumn("InputPlaceholder").AsString(_255).NotNullable()
                .WithColumn("HiddenInputName").AsAnsiString(_255).NotNullable()
                .WithColumn("HiddenTokenName").AsAnsiString(_255).NotNullable()
                .WithColumn("InputName").AsAnsiString(_255).NotNullable()
                .WithColumn("InputClass").AsAnsiString(_255).NotNullable()
                .WithColumn("InputTemplate").AsString(int.MaxValue)
                .WithColumn("Identifier").AsAnsiString(_255).NotNullable();
        }

        private void CreateOperationType()
        {
            Create.Table(nameof(TableName.OperationType))
               .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(TableName.OperationType))
               .WithColumn("Title").AsString(_255)
               .WithColumn("Css").AsString(_1023);
        }
        private void CreateDeepLog()
        {
            Create.Table(nameof(TableName.DeepLog))
               .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(TableName.DeepLog)).Identity()
               .WithColumn($"{nameof(TableName.OperationType)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.OperationType, TableName.DeepLog), nameof(TableName.OperationType), Id)
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
            Create.Table(nameof(TableName.User))
                .WithColumn(Id).AsGuid().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("FullName").AsString(_255).NotNullable()
                .WithColumn("DisplayName").AsString(_255).NotNullable()
                .WithColumn("Username").AsString(_255).Unique(NamingHelper.Uq(table,"Username")).NotNullable()
                .WithColumn("Password").AsString(int.MaxValue).NotNullable()
                .WithColumn("Mobile").AsAnsiString(11).NotNullable()
                .WithColumn("MobileConfirmed").AsBoolean().NotNullable()    
                .WithColumn("HasTwoStepVerification").AsBoolean().NotNullable()
                .WithColumn("InvalidLoginAttemptCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("LatestLoginDateTime").AsDateTime().Nullable()
                .WithColumn("LockTimespan").AsDateTime().Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateRole()
        {
            var table = TableName.Role;
            Create.Table(nameof(TableName.Role))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Name").AsAnsiString(_255).NotNullable() 
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn(Hash).AsString(_1023).NotNullable();
        }
        private void CreateUserRole()
        {
            var table = TableName.UserRole;
            Create.Table(nameof(TableName.UserRole))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table))                    
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), nameof(TableName.User), Id)
                .WithColumn($"{nameof(TableName.Role)}{Id}").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Role, TableName.UserRole), nameof(TableName.Role), Id)
                .WithColumn(Hash).AsString(_1023).NotNullable();
        }
        private void CreateUserToken()
        {
            var table = TableName.UserToken;
            Create.Table(nameof(TableName.UserToken))
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), nameof(TableName.User), Id)
                .WithColumn("AccessTokenExpiresDateTime").AsDate().NotNullable()
                .WithColumn("AccessTokenHash").AsString(_1023).NotNullable()
                .WithColumn("RefreshTokenExpiresDateTime").AsDateTime().NotNullable()
                .WithColumn("RefreshTokenIdHash").AsString(_1023)
                .WithColumn("RefreshTokenIdHashSource").AsString(_1023);
        }
        private void CreateUserLogin()
        {            
            var table = TableName.UserLogin;
            Create.Table(nameof(TableName.UserLogin))
                .WithColumn(Id).AsInt64().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Username").AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.User)}{Id}").AsGuid().Nullable()
                    .ForeignKey(NamingHelper.Fk(TableName.User, table), nameof(TableName.User), Id)
                .WithColumn("FirstStepDateTime").AsDateTime().NotNullable()
                .WithColumn("Ip").AsAnsiString(15).NotNullable()
                .WithColumn("FirstStepSuccess").AsBoolean().NotNullable()
                .WithColumn("WrongPassword").AsString(_1023).Nullable()
                .WithColumn("AppVersion").AsString(15).NotNullable()
                .WithColumn("TwoStepCode").AsAnsiString(15).Nullable()
                .WithColumn("TwoStepExpireDateTime").AsDateTime().Nullable()
                .WithColumn("TwoStepInsertDateTime").AsDateTime().Nullable()
                .WithColumn("TwoStepWasSuccessful").AsBoolean().Nullable()
                .WithColumn("PreviousFailureIsShown").AsBoolean().NotNullable()
                .WithColumn("LogoutDateTime").AsDateTime().Nullable()
                .WithColumn("LogoutReasonId").AsInt32().Nullable();

            //todo invalidloginRason table with enum id
            //todo logoutReason table with enum id
        }
    }
}