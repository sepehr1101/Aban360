using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts
{
    public interface ISmsByQueueIdGetHandler
    {
        Task<TrackingSmsDataOutputDto> Handle(Guid id, CancellationToken cancellationToken);
    }
}
