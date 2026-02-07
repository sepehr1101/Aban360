using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts
{
    public interface ITrackNumberAndDescriptionDetailHandler
    {
        Task<TrackNumberAndDescriptionOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken);
    }
}
