using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
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
