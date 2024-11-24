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
        public void SeedData()
        {
            if (!_users.Any())
            {
                var admin = new User()
                {
                     DisplayName="admin",
                     FullName="admin",
                     HasTwoStepVerification=false,
                     Id=Guid.NewGuid(),
                     InsertLogInfo=
                };
                _users.Add(admin);
                _uow.SaveChanges();
            }           
        }
    }
}
