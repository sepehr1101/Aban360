using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualEstateRelationTypeQueryService
    {
        Task<IndividualEstateRelationType> Get(short id);
        Task<ICollection<IndividualEstateRelationType>> Get();
    }
}
