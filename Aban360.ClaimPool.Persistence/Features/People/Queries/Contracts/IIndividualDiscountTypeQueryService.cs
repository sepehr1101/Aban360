using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualDiscountTypeQueryService
    {
        Task<IndividualDiscountType> Get(int id);
        Task<ICollection<IndividualDiscountType>> Get();
    }
}
