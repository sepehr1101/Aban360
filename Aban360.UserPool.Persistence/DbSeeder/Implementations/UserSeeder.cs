using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class UserSeeder : IDataSeeder
    {
        public int Order { get; set; } = 4;

        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        public UserSeeder(IUnitOfWork uow)
        {
            _uow= uow;
            _uow.NotNull(nameof(uow));

            _users=_uow.Set<User>();
            _users.NotNull(nameof(_users));
        }
        public async void SeedData()
        {
            if (!_users.Any())
            {
                var admin = new User()
                {
                    DisplayName = "programmer",
                    FullName = "programmer",
                    Username = "programmer",
                    HasTwoStepVerification = false,
                    Id = Guid.NewGuid(),
                    InsertLogInfo = LogInfoJson.Get(),
                    InvalidLoginAttemptCount = 0,
                    Mobile = "09130000000",
                    MobileConfirmed = false,
                    ValidFrom = DateTime.Now,
                    Password = await SecurityOperations.GetSha512Hash("123456"),
                    Hash = string.Empty
                };
                _users.Add(admin);
                _uow.SaveChanges();
            }           
        }
    }
}