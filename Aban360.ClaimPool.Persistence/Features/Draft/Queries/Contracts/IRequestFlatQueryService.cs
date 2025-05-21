using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestFlatQueryService
    {
        Task<RequestFlat> Get(int id);
        Task<ICollection<RequestFlat>> GetByEstateId(int id);
        Task<ICollection<RequestFlat>> Get();
    }
}
