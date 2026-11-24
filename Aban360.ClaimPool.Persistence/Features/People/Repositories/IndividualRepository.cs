using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Repositories
{
    internal sealed class IndividualRepository : IIndividualCommandService, IIndividualQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Individual> _individuals;
        public IndividualRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individuals = _uow.Set<Individual>();
            _individuals.NotNull(nameof(_individuals));
        }

        public async Task Add(Individual individual)
        {
            await _individuals.AddAsync(individual);
        }
        
        public async Task Add(ICollection<Individual> individuals)
        {
            await _individuals.AddRangeAsync(individuals);
        }

        public async Task Remove(Individual individual)
        {
            _individuals.Remove(individual);
        }

        public async Task<Individual> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Individual>(id);
        }

        public async Task<ICollection<Individual>> Get()
        {
            return await _individuals.ToListAsync();
        }

    }
}
