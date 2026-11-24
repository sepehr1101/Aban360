using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Repositories
{
    internal sealed class IndividualTagRepository : IIndividualTagCommandService, IIndividualTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualTag> _individualTag;
        public IndividualTagRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualTag = _uow.Set<IndividualTag>();
            _individualTag.NotNull(nameof(_individualTag));
        }

        public async Task Add(IndividualTag individualTag)
        {
            await _individualTag.AddAsync(individualTag);
        }

        public async Task Remove(IndividualTag individualTag)
        {
            _individualTag.Remove(individualTag);
        }

        public async Task<ICollection<IndividualTag>> Get(string nationalId)
        {
            return await _individualTag
                 .Include(w => w.Individual)
                 .Where(w => w.Individual.NationalId == nationalId)
                 .ToListAsync();
        }
        
        public async Task<IndividualTag> Get(int id)
        {
            return await _uow.FindOrThrowAsync<IndividualTag>(id);
        }

    }
}
