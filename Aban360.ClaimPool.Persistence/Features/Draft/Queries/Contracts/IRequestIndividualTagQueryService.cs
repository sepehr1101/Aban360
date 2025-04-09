using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestIndividualTagQueryService
    {
        Task<RequestIndividualTag> Get(int id);
        Task<ICollection<RequestIndividualTag>> Get();
    }
}
