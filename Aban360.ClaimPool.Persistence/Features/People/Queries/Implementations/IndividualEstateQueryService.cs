using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class IndividualEstateQueryService : IIndividualEstateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualEstate> _individualEstates;
        public IndividualEstateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualEstates = _uow.Set<IndividualEstate>();
            _individualEstates.NotNull(nameof(_individualEstates));
        }

        public async Task<IndividualEstate> Get(int id)
        {
            return await _uow.FindOrThrowAsync<IndividualEstate>(id);
        }

        public async Task<ICollection<IndividualEstate>> Get()
        {
            return await _individualEstates.ToListAsync();
        }
    }
}
