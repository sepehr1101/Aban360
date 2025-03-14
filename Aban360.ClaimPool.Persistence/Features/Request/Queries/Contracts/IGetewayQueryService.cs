using Aban360.ClaimPool.Domain.Features.Request.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IGetewayQueryService
    {
        Task<Geteway> Get(short id);
        Task<ICollection<Geteway>> Get();
    }
}