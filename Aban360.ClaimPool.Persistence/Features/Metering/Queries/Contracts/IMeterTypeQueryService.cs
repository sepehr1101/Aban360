using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IMeterTypeQueryService
    {
        Task<MeterType> Get(short id);
        Task<ICollection<MeterType>> Get();
    }
}
