using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IUseStateQueryService
    {
        Task<UseState> Get(UseStateEnum id);
        Task<ICollection<UseState>> Get();
    }
}
