using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts
{
    public interface ISeenByAssessmentHandler
    {
        Task<SeenByAssessmentOutputDto> Handle(TrackingDetailInputDto input, CancellationToken cancellationToken);
    }
}
