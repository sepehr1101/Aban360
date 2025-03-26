using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Implementations
{
    internal sealed class UserLeaveCommandService : IUserLeaveCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserLeave> _userLeave;
        public UserLeaveCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userLeave = _uow.Set<UserLeave>();
            _userLeave.NotNull(nameof(_userLeave));
        }

        public async Task Add(UserLeave userLeave)
        {
            await _userLeave.AddAsync(userLeave);
        }

        public async Task Remove(UserLeave userLeave)
        {
            _userLeave.Remove(userLeave);
        }
    }
}
