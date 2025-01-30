using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.Common.Db.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class LogoutReasonSeeder : IDataSeeder
    {
        public int Order { get; set; } = 8;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LogoutReason> _logoutReasons;
        public LogoutReasonSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _logoutReasons = _uow.Set<LogoutReason>();
            _logoutReasons.NotNull(nameof(_logoutReasons));
        }

        public void SeedData()
        {
            if (!_logoutReasons.Any())
            {
                var ByUser = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ByUser,
                    Title = MessageResources.ByUser
                };
                var ByAdmin = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ByAdmin,
                    Title = MessageResources.ByAdmin
                };
                var PasswordChange = new LogoutReason()
                {
                    Id = LogoutReasonEnum.PasswordChange,
                    Title = MessageResources.PasswordChange
                };
                var EditByAdmin = new LogoutReason()
                {
                    Id = LogoutReasonEnum.EditByAdmin,
                    Title = MessageResources.EditByAdmin
                };
                var ExpiredToken = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ExpiredToken,
                    Title = MessageResources.ExpiredToken
                };
                var ChangeIpInSession = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ChangeIpInSession,
                    Title = MessageResources.ChangeIpInSession
                };
                var ChangeClientMeta = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ChangeClientMeta,
                    Title = MessageResources.ChangeClientMeta
                };
                var ConcurrentLogin = new LogoutReason()
                {
                    Id = LogoutReasonEnum.ConcurrentLogin,
                    Title = MessageResources.ConcurrentLogin
                };

                var logoutReasons = new LogoutReason[]
                {
                    ByUser,
                    ByAdmin,
                    PasswordChange,
                    EditByAdmin,
                    ExpiredToken,
                    ChangeIpInSession,
                    ChangeClientMeta,
                    ConcurrentLogin,
                };

                _logoutReasons.AddRange(logoutReasons);
                _uow.SaveChanges();
            }
        }
    }
}
