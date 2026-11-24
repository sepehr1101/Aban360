using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Repositories
{
    internal sealed class IndividualTypeRepository : IIndividualTypeCommandService, IIndividualTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualType> _individualTypes;
        public IndividualTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualTypes = _uow.Set<IndividualType>();
            _individualTypes.NotNull(nameof(_individualTypes));
        }

        public async Task Add(IndividualType individual)
        {
            await _individualTypes.AddAsync(individual);
        }

        public async Task Remove(IndividualType individual)
        {
            _individualTypes.Remove(individual);
        }

        public async Task<IndividualType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<IndividualType>(id);
        }

        public async Task<ICollection<IndividualType>> Get()
        {
            return await _individualTypes.ToListAsync();
        }

    }
}
