using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestTrackingQueryService
    {
        Task<RequestTracking> Get(int id);
        Task<ICollection<RequestTracking>> GetByWaterMeterId(int id);
        Task<ICollection<RequestTracking>> Get();
    }
}