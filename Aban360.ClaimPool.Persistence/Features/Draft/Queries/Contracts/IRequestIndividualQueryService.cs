using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestIndividualQueryService
    {
        Task<RequestIndividual> Get(int id);
        Task<ICollection<RequestIndividual>> Get();
    }
}
