using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class UserLeaveQueryService : IUserLeaveQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserLeave> _userLeave;
        public UserLeaveQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userLeave = _uow.Set<UserLeave>();
            _userLeave.NotNull(nameof(_userLeave));
        }

        public async Task<UserLeave> Get(short id)
        {
            return await _uow.FindOrThrowAsync<UserLeave>(id);
        }

        public async Task<ICollection<UserLeave>> Get()
        {
            return await _userLeave.ToListAsync();
        }
    }
}
