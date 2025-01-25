using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public sealed class UserLoginCommandService : IUserLoginCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserLogin> _userLogins;
        public UserLoginCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLogins = _uow.Set<UserLogin>();
            _userLogins.NotNull(nameof(_userLogins));
        }
        public async Task Add(UserLogin userLogin)
        {
            await _userLogins.AddAsync(userLogin);
        }
    }
}
