using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        public UserCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _users = _uow.Set<User>();
            _users.NotNull(nameof(_users));
        }
        public async Task Add(User user)
        {
            await _users.AddAsync(user);
        }

        public void Remove(User user, string logInfo)
        {
            user.RemoveLogInfo = logInfo;
            user.ValidTo = DateTime.Now;
        }
    }
}