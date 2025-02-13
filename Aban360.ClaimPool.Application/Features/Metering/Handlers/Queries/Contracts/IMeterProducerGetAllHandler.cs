using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IMeterProducerGetAllHandler
    {
        Task<ICollection<MeterProducerGetDto>> Handle(CancellationToken cancellationToken);
    }

}
