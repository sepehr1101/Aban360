using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    public class HeadquarterCommandService : IHeadquarterCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Headquarters> _headquarter;
        public HeadquarterCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _headquarter = _uow.Set<Headquarters>();
            _headquarter.NotNull(nameof(_headquarter));

        }

        public async Task Add(Headquarters headquarter)
        {
            await _headquarter.AddAsync(headquarter);
        }

        public async Task Remove(Headquarters headquarter)
        {
            _headquarter.Remove(headquarter);
        }
    }
}
