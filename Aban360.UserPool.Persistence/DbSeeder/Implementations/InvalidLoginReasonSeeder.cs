using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class InvalidLoginReasonSeeder : IDataSeeder
    {
        public int Order { get; set; } = 6;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvalidLoginReason> _invalidLoginReason;
        public InvalidLoginReasonSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _invalidLoginReason = _uow.Set<InvalidLoginReason>();
            _invalidLoginReason.NotNull(nameof(_invalidLoginReason));
        }
        public void SeedData()
        {
            if (!_invalidLoginReason.Any())
            {
                var InvalidUsername = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.InvalidUsername,
                    Title = MessageResources.InvalidUsername
                };
                var InvalidPassword = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.InvalidPassword,
                    Title = MessageResources.InvalidPassword
                };
                var InvalidVerificationCode = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.InvalidVerificationCode,
                    Title = MessageResources.InvalidVerificationCode
                };
                var ExpiredVerificationCode = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.ExpiredVerificationCode,
                    Title = MessageResources.ExpiredVerificationCode
                };
                var LockedUser = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.LockedUser,
                    Title = MessageResources.LockedUser
                };
                var InactiveUser = new InvalidLoginReason()
                {
                    Id = InvalidLoginReasonEnum.InactiveUser,
                    Title = MessageResources.InactiveUser
                };

                var invalidLoginReasons = new InvalidLoginReason[]
                {
                    InvalidUsername,
                    InvalidPassword,
                    InvalidVerificationCode,
                    ExpiredVerificationCode,
                    LockedUser,
                    InactiveUser,
                };

                _invalidLoginReason.AddRange(invalidLoginReasons);
                _uow.SaveChanges();
            }
        }
    }
}
