using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class CordinalDirectionQueryService : ICordinalDirectionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CordinalDirection> _cordinalDirections;
        public CordinalDirectionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _cordinalDirections = _uow.Set<CordinalDirection>();
            _cordinalDirections.NotNull(nameof(_cordinalDirections));
        }

        public async Task<CordinalDirection> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CordinalDirection>(id);
        }

        public async Task<ICollection<CordinalDirection>> Get()
        {
            return await _cordinalDirections.ToListAsync();
        }
    }
}
