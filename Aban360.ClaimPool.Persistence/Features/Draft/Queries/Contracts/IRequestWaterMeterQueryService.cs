using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestWaterMeterQueryService
    {
        Task<RequestWaterMeter> Get(int id);
        Task<RequestWaterMeter> Get(string id);
        Task<ICollection<RequestWaterMeter>> Get();
    }
}
