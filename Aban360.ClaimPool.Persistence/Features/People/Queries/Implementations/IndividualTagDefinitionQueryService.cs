using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class IndividualTagDefinitionQueryService : IIndividualTagDefinitionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualTagDefinition> _individualTagDefinitions;
        public IndividualTagDefinitionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualTagDefinitions = _uow.Set<IndividualTagDefinition>();
            _individualTagDefinitions.NotNull(nameof(_individualTagDefinitions));
        }

        public async Task<IndividualTagDefinition> Get(short id)
        {
            return await _uow.FindOrThrowAsync<IndividualTagDefinition>(id);
        }

        public async Task<ICollection<IndividualTagDefinition>> Get()
        {
            return await _individualTagDefinitions.ToListAsync();
        }
    }
}
