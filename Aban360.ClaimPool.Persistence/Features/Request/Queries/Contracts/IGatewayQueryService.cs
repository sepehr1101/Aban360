using Aban360.ClaimPool.Domain.Features.Request.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IGatewayQueryService
    {
        Task<Gateway> Get(short id);
        Task<ICollection<Gateway>> Get();
    }
}