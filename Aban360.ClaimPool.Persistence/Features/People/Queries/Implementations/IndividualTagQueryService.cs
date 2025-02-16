using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    public class IndividualTagQueryService : IIndividualTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualTag> _IndividualTag;
        public IndividualTagQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _IndividualTag = _uow.Set<IndividualTag>();
            _IndividualTag.NotNull(nameof(_IndividualTag));
        }

        public async Task<ICollection<IndividualTag>> Get(string nationalId)
        {
            return await _IndividualTag
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
