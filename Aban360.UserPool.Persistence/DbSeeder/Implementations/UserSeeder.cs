using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.Common.Db.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class UserSeeder : IDataSeeder
    {
        public int Order { get; set; } = 6;
        public ClaimType ClaimType { get; private set; }

        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        public UserSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _users = _uow.Set<User>();
            _users.NotNull(nameof(_users));
        }
        public async void SeedData()
        {
            if (!_users.Any())
            {
                //var logInfo = LogInfoJson.Get();
                //var userId = Guid.NewGuid();
                //var programmer = new User()
                //{
                //    DisplayName = "برنامه نویس",
                //    FullName = "برنامه نویس",
                //    Username = "programmer",
                //    HasTwoStepVerification = false,
                //    Id = userId,
                //    InsertLogInfo = logInfo,
                //    InvalidLoginAttemptCount = 0,
                //    Mobile = "09130000000",
                //    MobileConfirmed = false,
                //    ValidFrom = DateTime.Now,
                //    Password = await SecurityOperations.GetSha512Hash("123456"),
                //    Hash = string.Empty,
                //    SerialNumber = Guid.NewGuid().ToString("n"),
                //    UserRoles = GetProgrammerRoles(userId, logInfo),
                //    UserClaims = GetProgrammerUserClaims(userId, logInfo)
                //};

                ICollection<User> users = new List<User>();
                users.Add(await GetUser("1برنامه نویس", "برنامه نویس1", "programmer1"));
                users.Add(await GetUser("برنامه نویس2", "برنامه نویس2", "programmer2"));
                users.Add(await GetUser("برنامه نویس3", "برنامه نویس3", "programmer3"));
                users.Add(await GetUser("برنامه نویس4", "برنامه نویس4", "programmer4"));

                _users.AddRange(users);
                _uow.SaveChanges();
            }
        }
        private ICollection<UserRole> GetProgrammerRoles(Guid userId, string logInfo)
        {
            var userRoles = new List<UserRole>()
            {
                new UserRole() {Hash=string.Empty,UserId=userId,InsertGroupId=Guid.NewGuid(),RoleId=3,ValidFrom=DateTime.Now,InsertLogInfo=logInfo},
            };
            return userRoles;
        }
        private ICollection<UserClaim> GetProgrammerUserClaims(Guid userid, string logInfo)
        {
            var insertGroupId = Guid.NewGuid();
            var now = DateTime.Now;
            var userClaims = new List<UserClaim>()
            {
                new UserClaim() { Hash = string.Empty, ClaimTypeId = ClaimType.DefaultZoneId, ClaimValue="131301", UserId = userid, InsertGroupId =insertGroupId,InsertLogInfo=logInfo,ValidFrom=now},
                new UserClaim() { Hash = string.Empty, ClaimTypeId = ClaimType.Endpoint, ClaimValue=@"UserCreate.Trigger", UserId = userid, InsertGroupId =insertGroupId,InsertLogInfo=logInfo,ValidFrom=now},
                new UserClaim() { Hash = string.Empty, ClaimTypeId = ClaimType.Endpoint, ClaimValue=@"CaptchaDisplayMode.Get", UserId = userid, InsertGroupId =insertGroupId,InsertLogInfo=logInfo,ValidFrom=now},
                new UserClaim() { Hash = string.Empty, ClaimTypeId = ClaimType.Endpoint, ClaimValue=@"CaptchaLanguage.Get", UserId = userid, InsertGroupId =insertGroupId,InsertLogInfo=logInfo,ValidFrom=now},
                new UserClaim() { Hash = string.Empty, ClaimTypeId = ClaimType.Endpoint, ClaimValue=@"CaptchaDictionary.Get", UserId = userid, InsertGroupId =insertGroupId,InsertLogInfo=logInfo,ValidFrom=now},
            };
            return userClaims;
        }
        private async Task<User> GetUser(string displayName, string fullname, string userName)
        {
            var logInfo = LogInfoJson.Get();
            var userId = Guid.NewGuid();
            var programmer = new User()
            {
                DisplayName = displayName,
                FullName = fullname,
                Username = userName,
                HasTwoStepVerification = false,
                Id = userId,
                InsertLogInfo = logInfo,
                InvalidLoginAttemptCount = 0,
                Mobile = "09130000000",
                MobileConfirmed = false,
                ValidFrom = DateTime.Now,
                Password = await SecurityOperations.GetSha512Hash("123456"),
                Hash = string.Empty,
                SerialNumber = Guid.NewGuid().ToString("n"),
                UserRoles = GetProgrammerRoles(userId, logInfo),
                UserClaims = GetProgrammerUserClaims(userId, logInfo)

            };
            return programmer;
        }
    }
}