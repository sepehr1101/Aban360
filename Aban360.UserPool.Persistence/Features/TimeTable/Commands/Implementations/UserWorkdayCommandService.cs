using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Implementations
{
    internal sealed class UserWorkdayCommandService : IUserWorkdayCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserWorkday> _userWorkday;
        public UserWorkdayCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userWorkday = _uow.Set<UserWorkday>();
            _userWorkday.NotNull(nameof(_userWorkday));
        }

        public async Task Add(UserWorkday userWorkday)
        {
            await _userWorkday.AddAsync(userWorkday);
        }

        public async Task Remove(UserWorkday userWorkday)
        {
            _userWorkday.Remove(userWorkday);
        }
    }
}
