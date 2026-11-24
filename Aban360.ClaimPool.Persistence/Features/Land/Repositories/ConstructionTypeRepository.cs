using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class ConstructionTypeRepository : IConstructionTypeCommandService, IConstructionTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ConstructionType> _constructionTypes;
        public ConstructionTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _constructionTypes = _uow.Set<ConstructionType>();
            _constructionTypes.NotNull(nameof(_constructionTypes));
        }

        public async Task Add(ConstructionType constructionType)
        {
            await _constructionTypes.AddAsync(constructionType);
        }

        public async Task Remove(ConstructionType constructionType)
        {
            _constructionTypes.Remove(constructionType);
        }

        public async Task<ConstructionType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<ConstructionType>(id);
        }

        public async Task<ICollection<ConstructionType>> Get()
        {
            return await _constructionTypes.ToListAsync();
        }

    }
}
