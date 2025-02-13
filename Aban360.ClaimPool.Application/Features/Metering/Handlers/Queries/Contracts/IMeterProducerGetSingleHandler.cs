using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IMeterProducerGetSingleHandler
    {
        Task<MeterProducerGetDto> Handle(short id, CancellationToken cancellationToken);
    }

}
