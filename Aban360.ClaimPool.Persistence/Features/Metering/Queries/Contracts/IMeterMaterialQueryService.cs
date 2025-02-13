using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IMeterMaterialQueryService
    {
        Task<MeterMaterial> Get(short id);
        Task<ICollection<MeterMaterial>> Get();
    }
}
