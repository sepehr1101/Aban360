using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IDiscountTypeQueryService
    {
        Task<DiscountType> Get(DiscountTypeEnum id);
        Task<ICollection<DiscountType>> Get();
    }
}
