using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestIndividualDiscountTypeQueryService
    {
        Task<RequestIndividualDiscountType> Get(int id);
        Task<ICollection<RequestIndividualDiscountType>> Get();
    }
}
