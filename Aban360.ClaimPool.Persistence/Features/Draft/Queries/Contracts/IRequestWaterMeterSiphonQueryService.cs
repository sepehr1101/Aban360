using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestWaterMeterSiphonQueryService
    {
        Task<RequestWaterMeterSiphon> Get(int id);
        Task<RequestWaterMeterSiphon> GetBySiphonId(int id);
        Task<ICollection<RequestWaterMeterSiphon>> Get();
        Task<ICollection<RequestWaterMeterSiphon>> GetByWaterMeterId(int waterMeterId);
    }
}
