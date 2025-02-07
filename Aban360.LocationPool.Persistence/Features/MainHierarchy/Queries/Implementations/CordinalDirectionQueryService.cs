using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class CordinalDirectionQueryService : ICordinalDirectionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CordinalDirection> _direction;
        public CordinalDirectionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _direction = _uow.Set<CordinalDirection>();
            _direction.NotNull(nameof(_direction));
        }

        public async Task<CordinalDirection> Get(short id)
        {
            return _uow.FindOrThrow<CordinalDirection>(id);
        }

        public async Task<ICollection<CordinalDirection>> Get()
        {
            return await _direction.ToListAsync();
        }
    }
}
