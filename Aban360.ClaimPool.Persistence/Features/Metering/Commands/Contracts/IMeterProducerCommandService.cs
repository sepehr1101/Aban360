using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IMeterProducerCommandService
    {
        Task Add(MeterProducer meterProducer);
        Task Remove(MeterProducer meterProducer);
    }
}
