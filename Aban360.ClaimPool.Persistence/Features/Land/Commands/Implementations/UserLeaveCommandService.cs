using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
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
