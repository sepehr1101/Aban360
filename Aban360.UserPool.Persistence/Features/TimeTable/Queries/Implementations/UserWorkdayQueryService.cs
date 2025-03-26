using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class UserWorkdayQueryService : IUserWorkdayQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserWorkday> _userWorkday;
        public UserWorkdayQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userWorkday = _uow.Set<UserWorkday>();
            _userWorkday.NotNull(nameof(_userWorkday));
        }

        public async Task<UserWorkday> Get(short id)
        {
            return await _uow.FindOrThrowAsync<UserWorkday>(id);
        }

        public async Task<ICollection<UserWorkday>> Get()
        {
            return await _userWorkday.ToListAsync();
        }
    }
}
