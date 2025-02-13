using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IMeterDiameterQueryService
    {
        Task<MeterDiameter> Get(short id);
        Task<ICollection<MeterDiameter>> Get();
    }
}
