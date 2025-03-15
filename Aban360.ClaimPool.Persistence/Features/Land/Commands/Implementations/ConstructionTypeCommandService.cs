using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class ConstructionTypeCommandService : IConstructionTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ConstructionType> _constructionTypes;
        public ConstructionTypeCommandService(IUnitOfWork uow)
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
    }
}
