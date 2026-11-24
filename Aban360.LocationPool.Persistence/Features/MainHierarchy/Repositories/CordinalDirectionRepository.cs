using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Repositories
{
    internal sealed class CordinalDirectionRepository : ICordinalDirectionCommandService, ICordinalDirectionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CordinalDirection> _directions;
        public CordinalDirectionRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _directions = _uow.Set<CordinalDirection>();
            _directions.NotNull(nameof(_directions));
        }

        public async Task Add(CordinalDirection cordinalDirection)
        {
            await _directions.AddAsync(cordinalDirection);
        }

        public async Task Remove(CordinalDirection cordinalDirection)
        {
            _directions.Remove(cordinalDirection);
        }

        public async Task<CordinalDirection> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CordinalDirection>(id);
        }

        public async Task<ICollection<CordinalDirection>> Get()
        {
            return await _directions.ToListAsync();
        }

    }
}
