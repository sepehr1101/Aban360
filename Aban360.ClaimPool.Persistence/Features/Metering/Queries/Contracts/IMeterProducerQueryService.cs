using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IMeterProducerQueryService
    {
        Task<MeterProducer> Get(short id);
        Task<ICollection<MeterProducer>> Get();
    }
}
