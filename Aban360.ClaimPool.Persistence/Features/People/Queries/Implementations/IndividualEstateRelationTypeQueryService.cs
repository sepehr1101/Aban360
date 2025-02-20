using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    public class IndividualEstateRelationTypeQueryService : IIndividualEstateRelationTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualEstateRelationType> _individualEstateRelationTypes;
        public IndividualEstateRelationTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualEstateRelationTypes = _uow.Set<IndividualEstateRelationType>();
            _individualEstateRelationTypes.NotNull(nameof(_individualEstateRelationTypes));
        }

        public async Task<IndividualEstateRelationType> Get(IndividualEstateRelationEnum id)
        {
            return await _uow.FindOrThrowAsync<IndividualEstateRelationType>(id);
        }

        public async Task<ICollection<IndividualEstateRelationType>> Get()
        {
            return await _individualEstateRelationTypes.ToListAsync();
        }
    }
}
