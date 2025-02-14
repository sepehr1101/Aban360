using Aban360.ClaimPool.Domain.Features.Registration.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts
{
    public interface IUseStateQueryService
    {
        Task<UseState> Get(short id);
        Task<ICollection<UseState>> Get();
    }
}
